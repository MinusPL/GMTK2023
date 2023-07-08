using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _width = 42;
    [SerializeField]
    private int _height = 30;
    [SerializeField]
    private Transform _tilesGroup;
    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private float _roundTime = 180.0f;

    private Grid _worldGrid;
    private Dictionary<Vector2Int, GameObject> _tiles;

    private Dictionary<PlayerController, int> _score;

    private InGameUIController _uiController;
    // Start is called before the first frame update
    void Start()
    {
        _tiles = new Dictionary<Vector2Int, GameObject>();
        _worldGrid = GameObject.FindGameObjectWithTag("WorldGrid").GetComponent<Grid>();
        _uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();
        PrepareLevelGrid();
        StartRound();
        _score = new Dictionary<PlayerController, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PrepareLevelGrid()
    {
        for(int i = -_width/2; i < _width/2; i++)
        {
            for (int j = -_height / 2; j < _height / 2; j++)
            {
                var tileObj = GameObject.Instantiate(_tilePrefab);
                tileObj.transform.position = _worldGrid.CellToWorld(new Vector3Int(i, j, 0));
                tileObj.name = string.Format("Tile [{0}, {1}]", i, j);
                tileObj.transform.parent = _tilesGroup;
                _tiles.Add(new Vector2Int(i, j), tileObj);
            }
        }
    }

    public void CheckTile(Vector3 position)
    {
        Vector3Int gridPos = _worldGrid.WorldToCell(position);
        _tiles[new Vector2Int(gridPos.x, gridPos.y)].SetActive(false);
    }

    private void StartRound()
    {
        _uiController.StartRoundTimer(_roundTime);
    }

    public void ChangePlayerScore(PlayerController player, int changeValue)
    {
        if (!_score.ContainsKey(player))
            _score.Add(player, 0);
        _score[player] += changeValue;
        _uiController.UpdateScores(_score);
    }
}
