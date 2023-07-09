using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [Header("References to UI elements")]
    [SerializeField]
    private TextMeshProUGUI _clockText;

    [SerializeField]
    private List<TextMeshProUGUI> _scoreTexts;
    [SerializeField]
    private List<Image> _playerImgs;

    [SerializeField]
    private TextMeshProUGUI _gumkaText;
    [SerializeField]
    private TextMeshProUGUI _inkShotText;

    [SerializeField]
    private Image _shotsImage;

    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject _tutorial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetRoundTimer(float time)
    {
        TimeSpan.FromSeconds(time);
        _clockText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
    }

    public void UpdateScores(Dictionary<int, int> scores)
    {
        foreach(var score in scores)
        {
            _scoreTexts[score.Key].text = String.Format("{0}", score.Value);
        }
    }

    public void SetPlayerColor(int playerIndex, Color color)
    {
        _playerImgs[playerIndex].color = color;
    }

    public void SetPlayerGumkaAmmo(int amount)
    {
        _gumkaText.text = String.Format("x {0:D2}", amount);
    }

    public void SetPlayerInkShotAmmo(int amount)
    {
        _inkShotText.text = String.Format("x {0:D2}", amount);
    }

    public void SetMainPlayerShotsColor(Color color)
    {
        _shotsImage.color = color;
    }

    public void ShowMenu(bool flag)
    {
        if(flag)
        {
            _menu.SetActive(true);
        }
        else
        {
            _menu.SetActive(false);
        }
    }

    public void HideTutorial()
    {
        _tutorial.SetActive(false);
    }
}

