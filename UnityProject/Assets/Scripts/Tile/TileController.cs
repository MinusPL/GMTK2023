using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public struct TileData
{
    public Color color;
    public Sprite image;
    public int ownerID;
}

[RequireComponent(typeof(BoxCollider2D))]
public class TileController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _spriteImages;
    [SerializeField]
    private List<SpriteRenderer> _sprites;
    private int _currentPlayerInRange = -1;
    private int _lastPlayerInTileQueue = -1;

    private Queue<TileData> _data;


    private GameManager _gameManager;

    public int ownerID = -1;


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
            if (_currentPlayerInRange == -1)
            {
                _currentPlayerInRange = collision.transform.parent.gameObject.GetComponent<PlayerController>().ID;
                ownerID = _currentPlayerInRange;
                //Notify game manager, change scoring, set tile image and color to something
                UpdateTile(collision.transform.parent.gameObject.GetComponent<PlayerController>());
            }
        }
        else if(collision.CompareTag("Gumka"))
        {
            ClearTile();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerRange"))
        {
            if (_currentPlayerInRange != -1 && collision.transform.parent.gameObject.GetComponent<PlayerController>().ID == _currentPlayerInRange)
            {
                _currentPlayerInRange = -1;
            }
        }
    }

    public void ClearTile()
    {
        if (_lastPlayerInTileQueue != -1) _gameManager.ChangePlayerScore(_lastPlayerInTileQueue, -1);
        _lastPlayerInTileQueue = -1;
        _currentPlayerInRange = -1;
        _data.Clear();
        for(int i = 0; i < 4; i++)
        {
            _sprites[i].color = Color.white;
            _sprites[i].sprite = null;
        }
    }

    public void UpdateTile(PlayerController player)
    {
        if (_lastPlayerInTileQueue == _currentPlayerInRange)
            return;

        if(_lastPlayerInTileQueue != -1) _gameManager.ChangePlayerScore(_lastPlayerInTileQueue, -1);
        _gameManager.ChangePlayerScore(_currentPlayerInRange, 1);
        _lastPlayerInTileQueue = _currentPlayerInRange;
        //Insert new data and drop last one if needed
        TileData newData = new()
        {
            color = player.GetPlayerColor(),
            image = _spriteImages[Random.Range(0, _spriteImages.Count)],
        };
        _data.Enqueue(newData);

        if (_data.Count > 4)
            _data.Dequeue();
        
        //Change Display
        int li = 0;
        foreach(var td in _data.Reverse())
        {
            _sprites[li].color = td.color;
            _sprites[li].sprite = td.image;
            li++;
        }
        
    }

    public void SetTile(PlayerController player)
    {
        if (_lastPlayerInTileQueue != -1) _gameManager.ChangePlayerScore(_lastPlayerInTileQueue, -1);
        _gameManager.ChangePlayerScore(player.ID, 1);
        _lastPlayerInTileQueue = player.ID;
        //Insert new data and drop last one if needed
        TileData newData = new()
        {
            color = player.GetPlayerColor(),
            image = _spriteImages[Random.Range(0, _spriteImages.Count)],
        };
        _data.Enqueue(newData);

        if (_data.Count > 4)
            _data.Dequeue();

        //Change Display
        int li = 0;
        foreach (var td in _data.Reverse())
        {
            _sprites[li].color = td.color;
            _sprites[li].sprite = td.image;
            li++;
        }

    }
}
