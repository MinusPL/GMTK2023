using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _gumka;
    [SerializeField]
    private float _moveSpeed = 5.0f;

    private Rigidbody2D _rb;

    private Vector2 _moveDirection;

    [SerializeField]
    private bool _aiControlled = false;

    [SerializeField]
    private Color _color = new() { r = 1.0f, g = 0.0f, b = 0.0f, a = 1.0f };

    [SerializeField]
    private SpriteRenderer _playerColorSprite;
    public int ID;


    [SerializeField]
    private Collider2D _rangeCollider;
    [SerializeField]
    private Collider2D _tilesDetectCollider;

    //AI stuff
    [SerializeField]
    private LayerMask _tileLayerMask;
    private int _state = 0;
    private Vector3 _targetPosition;

    [SerializeField]
    private int _ownMultiplier = 1;
    [SerializeField]
    private int _emptyMultiplier = 5;
    [SerializeField]
    private int _enemyMultiplier = 10;
    //private GameManager _gameManager;

    [SerializeField]
    private float _moveThreshold = 0.05f;
    [SerializeField]
    private float _escapeTime = 3.0f;
    private float _timer = 0.0f;
    private Vector3 _lastPos = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerColorSprite.color = _color;
        //_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(_aiControlled)
        {
            switch (_state)
            {
                case 0: //Choose what to do, should not stay in this state
                    _targetPosition = GetNextTargetPosition();

                    _timer = _escapeTime;
                    _state = 1;
                    break;
                case 1: //
                    if (Vector3.Distance(transform.position, _lastPos) > _moveThreshold)
                        _timer = _escapeTime;
                    
                    if(_timer <= 0.0f)
                        _state = 2;
                    if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
                    {
                        _moveDirection = (_targetPosition - transform.position).normalized;
                    }
                    else
                    {
                        transform.position = _targetPosition;
                        _state = 0;
                    }
                    _lastPos = transform.position;
                    break;
                case 2:
                    _targetPosition = GetNextTargetPosition(true);

                    _state = 1;

                    break;
            }
            if (_timer > 0.0f) _timer -= Time.deltaTime; 
        }
        else
        {

        }
    }

    private Vector3 GetNextTargetPosition(bool skipRandom = false)
    {
        int r = UnityEngine.Random.Range(0, 10000) + 1;
        if (r > 4000 && !skipRandom)
        {
            Collider2D[] colls = new Collider2D[24];
            var cf = new ContactFilter2D();
            cf.SetLayerMask(_tileLayerMask);
            _tilesDetectCollider.OverlapCollider(cf, colls);

            List<Collider2D> cl = colls.ToList();

            cl.RemoveAll(x => x == null);
            cl.RemoveAll(x => x.gameObject.CompareTag("TileTag") == false);
            cl.RemoveAll(x => x.transform.position.x == transform.position.x && x.transform.position.y == transform.position.y);

            List<Vector3> positions = new List<Vector3>();

            foreach (var c in cl)
            {
                int oid = c.gameObject.GetComponent<TileController>().ownerID;
                int i = _ownMultiplier;
                if (oid == -1)
                {
                    i = _emptyMultiplier;
                }
                else if (oid != ID)
                {
                    i = _enemyMultiplier;
                }
                for (; i > 0; i--)
                {
                    positions.Add(c.transform.position);
                }
            }
            return positions[UnityEngine.Random.Range(0, positions.Count)];
        }
        else
        {
            int x = UnityEngine.Random.Range(-21, 20);
            int y = UnityEngine.Random.Range(-15, 14);

            return new Vector3((float)x, (float)y, 0.0f);
        }
    }

    private void FixedUpdate()
    {
        //basically deadzone prevention
        if (_moveDirection.magnitude > 0.01f)
        {
            _rb.velocity = _moveDirection * _moveSpeed;
            //Move to physics processing and base on RigidBody velocity
        }
        else
            _rb.velocity = Vector2.zero;
    }

    public void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    public void OnGumShot(InputValue value)
    {
        SpawnGumka();
    }

    public Color GetPlayerColor()
    {
        return _color;
    }

    public void SetPlayerColor(Color color)
    {
        _color = color;
        _playerColorSprite.color = _color;
    }

    private void SpawnGumka()
    {
        var go = GameObject.Instantiate(_gumka);
        go.transform.position = transform.position;
        if (_moveDirection.magnitude < 0.01f)
            go.GetComponent<Gumka>().direction = Vector3.up;
        else
            go.GetComponent<Gumka>().direction = _moveDirection.normalized;
    }
}
