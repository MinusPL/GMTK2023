using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleRainbomizer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sr;
    [SerializeField]
    private float _colorTime = 2.0f;
    private float _colorTimer;
    // Start is called before the first frame update
    void Start()
    {
        _colorTimer = _colorTime;
    }

    // Update is called once per frame
    void Update()
    {
        _colorTimer -= Time.deltaTime;

        if (_colorTimer <= 0.0f) _colorTimer += _colorTime;

        Color c = _sr.color;
        Color.RGBToHSV(c, out float H, out float S, out float V);
        H = Mathf.Lerp(0.0f, 1.0f, Mathf.Sin((_colorTimer / _colorTime) * (Mathf.PI / 2)));
        _sr.color = Color.HSVToRGB(H, S, V);
    }
}
