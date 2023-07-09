using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXRemover : MonoBehaviour
{
    private AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_as.isPlaying) Destroy(gameObject);
    }
}
