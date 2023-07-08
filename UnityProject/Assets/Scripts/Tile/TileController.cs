using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileData
{
    public Color color;
    public Sprite image;
    public PlayerController playerRef;
}

[RequireComponent(typeof(BoxCollider2D))]
public class TileController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _spriteImages;
    [SerializeField]
    private List<SpriteRenderer> _sprites;
    private PlayerController _currentPlayerInRange = null;
    private PlayerController _lastPlayerInTileQueue = null;

    private Queue<TileData> _data;


    private GameManager _gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        _data = new Queue<TileData>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerRange"))
        {
            if (_currentPlayerInRange == null)
            {
                _currentPlayerInRange = collision.transform.parent.gameObject.GetComponent<PlayerController>();
                //Notify game manager, change scoring, set tile image and color to something
                UpdateTile();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerRange"))
        {
            if (_currentPlayerInRange != null && collision.transform.parent.gameObject.GetComponent<PlayerController>() == _currentPlayerInRange)
            {
                _currentPlayerInRange = null;
            }
        }
    }

    public void UpdateTile()
    {
        if (_lastPlayerInTileQueue == _currentPlayerInRange)
            return;

        if(_lastPlayerInTileQueue != null) _gameManager.ChangePlayerScore(_lastPlayerInTileQueue, -1);
        _gameManager.ChangePlayerScore(_currentPlayerInRange, 1);
        _lastPlayerInTileQueue = _currentPlayerInRange;
        //Insert new data and drop last one if needed
        TileData newData = new()
        {
            color = _currentPlayerInRange.GetPlayerColor(),
            image = _spriteImages[Random.Range(0, _spriteImages.Count)],
            playerRef = _currentPlayerInRange
        };
        _data.Enqueue(newData);

        if (_data.Count > 4)
            _data.Dequeue();
        
        //Change Display
        int li = 0;
        foreach(var td in _data)
        {
            _sprites[li].color = td.color;
            _sprites[li].sprite = td.image;
            li++;
        }
        
    }
}
