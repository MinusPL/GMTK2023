using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

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


    [SerializeField]
    private PickupSpawnArea _psa;
    [SerializeField] 
    private float _pickupSpawnMinTime = 5.0f;
    [SerializeField]
    private float _pickupSpawnMaxTime = 10.0f;
    [SerializeField]
    private List<GameObject> _pickups;

    [SerializeField]
    private AudioSource _bellSource;

    private float _pSpawnTimer = 0.0f;


    private float _roundTimer = 0.0f;


    public bool gameRunning = false;
    public bool tutorialRead = false;
    
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _tiles = new Dictionary<Vector2Int, GameObject>();
        _worldGrid = GameObject.FindGameObjectWithTag("WorldGrid").GetComponent<Grid>();
        _uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<InGameUIController>();
        PrepareLevelGrid();
        _score = new Dictionary<int, int>();
        foreach(var player in _players)
        {
            _uiController.SetPlayerColor(player.ID, player.GetPlayerColor());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
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


            if (_pSpawnTimer > 0.0f)
            {
                _pSpawnTimer -= Time.deltaTime;
                if (_pSpawnTimer <= 0.0f)
                {
                    _pSpawnTimer = Random.Range(_pickupSpawnMinTime, _pickupSpawnMaxTime) + _pSpawnTimer;
                    SpawnPickup();
                }
            }

            if(_roundTimer > 0.0f)
            {
                _roundTimer -= Time.deltaTime;
                if(_roundTimer <= 0.0f)
                {
                    EndRound();
                    _roundTimer = 0.0f;
                }
            }
            _uiController.SetRoundTimer(_roundTimer);
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

    public void StartRound()
    {
        _uiController.HideTutorial();
        _colorSwitchTimer = Random.Range(_minColorSwitchTime, _maxColorSwitchTime);
        _pSpawnTimer = _pSpawnTimer = Random.Range(_pickupSpawnMinTime, _pickupSpawnMaxTime);
        gameRunning = true;
        _roundTimer = _roundTime;
        tutorialRead = true;
    }

    private void EndRound()
    {
        gameRunning = false;

        foreach(var p in _players)
        {
            p.SetZeroVelocity();
        }
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
        _bellSource.Play();
    }

    public void UpdatePlayerGumka(int amount)
    {
        _uiController.SetPlayerGumkaAmmo(amount);
    }

    public void UpdatePlayerInkShot(int amount)
    {
        _uiController.SetPlayerInkShotAmmo(amount);
    }

    private void SpawnPickup()
    {
        if (_pickups.Count > 0)
        {
            Vector3 pickupPos = _psa.GetPoint();
            int i = Random.Range(0, _pickups.Count);
            var p = Instantiate(_pickups[i]);
            p.transform.position = pickupPos;
        }
    }

    public void UpdateMainPlayerColor(PlayerController player)
    {
        _uiController.SetMainPlayerShotsColor(player.GetPlayerColor());
    }

    public void ExitMenu()
    {
        gameRunning = true;
        _uiController.ShowMenu(false);
    }

    public void EnterMenu()
    {
        gameRunning = false;
        _uiController.ShowMenu(true);
        foreach (var p in _players)
        {
            p.SetZeroVelocity();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
