using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InkBomb : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _flyTime = 1.0f;
    private float _flyTimer = 0.0f;
    [SerializeField]
    private LayerMask _tileLayerMask;
    [SerializeField]
    private Collider2D _tilesDetectCollider;
    public PlayerController player;
    [SerializeField]
    private SpriteRenderer _ink;
    [SerializeField]
    private Vector3 _largerScale = new Vector3(1.5f,1.5f,1.5f);
    [SerializeField]
    private GameObject _sfx;

    public Vector3 direction = Vector3.right;
    // Start is called before the first frame update
    void Start()
    {
        _flyTimer = _flyTime;
        _ink.color = player.GetPlayerColor();
    }

    // Update is called once per frame
    void Update()
    {
        if(_flyTimer <= 0.0f)
        {
            //splash
            Collider2D[] colls = new Collider2D[24];
            var cf = new ContactFilter2D();
            cf.SetLayerMask(_tileLayerMask);
            _tilesDetectCollider.OverlapCollider(cf, colls);

            List<Collider2D> cl = colls.ToList();

            cl.RemoveAll(x => x == null);
            cl.RemoveAll(x => x.gameObject.CompareTag("TileTag") == false);

            foreach (var c in cl)
            {
                c.GetComponent<TileController>().SetTile(player);
            }
            var sfxo = Instantiate(_sfx);
            sfxo.transform.position = transform.position;
            //remove projectile
            Destroy(gameObject);
        }

        transform.localScale = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), _largerScale, Mathf.Sin((_flyTimer / _flyTime) * Mathf.PI));
    }

    private void FixedUpdate()
    {
        if (_flyTimer > 0.0f)
        {
            _flyTimer -= Time.fixedDeltaTime;
            transform.position += direction * _speed * Time.fixedDeltaTime;
        }
    }
}
