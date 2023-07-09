using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

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

    [SerializeField]
    private List<PlayerController> _players;
    private int _pc = 0;

    private Grid _worldGrid;
    private Dictionary<Vector2Int, GameObject> _tiles;

    private Dictionary<int, int> _score;

    private InGameUIController _uiController;

    [SerializeField]
    private InputActionAsset _controlsAsset;

    [SerializeField]
    private float _minColorSwitchTime = 5.0f;
    [SerializeField]
    private float _maxColorSwitchTime = 20.0f;

    private float _colorSwitchTimer = 0.0f;

    private void Awake()
    {
        //InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }

    // Start is called before the first frame update
    void Start()
    {
        _tiles = new Dictionary<Vector2Int, GameObject>();
        _worldGrid = GameObject.FindGameObjectWithTag("WorldGrid").GetComponent<Grid>();
        _uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();
        PrepareLevelGrid();
        StartRound();
        _score = new Dictionary<int, int>();

        foreach(var player in _players)
        {
            _uiController.SetPlayerColor(player.ID, player.GetPlayerColor());
        }

        //InputUser.listenForUnpairedDeviceActivity = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (_colorSwitchTimer > 0.0f)
        {
            _colorSwitchTimer -= Time.deltaTime;
            if (_colorSwitchTimer <= 0.0f)
            {
                _colorSwitchTimer = Random.Range(_minColorSwitchTime, _maxColorSwitchTime);
                SwitchColors();
            }
        }
    }

    private void OnDestroy()
    {
        //InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
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
        _colorSwitchTimer = Random.Range(_minColorSwitchTime, _maxColorSwitchTime);
    }

    public void ChangePlayerScore(int playerIndex, int changeValue)
    {
        if (!_score.ContainsKey(playerIndex))
            _score.Add(playerIndex, 0);
        _score[playerIndex] += changeValue;
        _uiController.UpdateScores(_score);
    }


    private void SwitchColors()
    {
        List<int> used = new List<int>();
        List<int> ids = new List<int>();
        List<Color> colors = new List<Color>();
        foreach (var player in _players)
        {
            ids.Add(player.ID);
            colors.Add(player.GetPlayerColor());
        }

        foreach(var player in _players)
        {
            int n;
            do
            {
                n = Random.Range(0, 4);
            } while (used.Contains(n));
            used.Add(n);
            player.ID = ids[n];
            player.SetPlayerColor(colors[n]);

        }
    }

    //void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr)
    //{
    //    if (!(control is ButtonControl))
    //        return;

    //    if (control.device is Mouse)
    //        return;

    //    PlayerInput pi = (PlayerInput)_players[_pc].gameObject.AddComponent(typeof(PlayerInput));
    //    pi.actions = _controlsAsset;
    //    pi.gameObject.SetActive(false);
    //    pi.gameObject.SetActive(true);
    //    pi.SwitchCurrentActionMap("Player");
    //    _pc++;
    //    InputUser.listenForUnpairedDeviceActivity--;
    //    Debug.Log("Unpaired device detected" + control.device.displayName);
    //}
}
