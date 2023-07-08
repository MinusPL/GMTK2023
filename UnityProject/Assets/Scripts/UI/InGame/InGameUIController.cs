using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    [Header("References to UI elements")]
    [SerializeField]
    private TextMeshProUGUI _clockText;
    private float _roundTimer = 0.0f;

    [SerializeField]
    private List<TextMeshProUGUI> _scoreTexts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Timers, logic and all that stuff that is not called from other scripts
        if (_roundTimer > 0.0f)
        {
            _roundTimer -= Time.deltaTime;
            if (_roundTimer < 0.0f) _roundTimer = 0.0f;
            TimeSpan.FromSeconds(_roundTimer);
            _clockText.text = TimeSpan.FromSeconds(_roundTimer).ToString(@"mm\:ss");
        }
    }

    public void StartRoundTimer(float time)
    {
        _roundTimer = time;
    }

    public void UpdateScores(Dictionary<PlayerController, int> scores)
    {
        int li = 0;
        foreach(var score in scores)
        {
            _scoreTexts[li].text = String.Format("{0}", score.Value);
            li++;
        }
    }
}
