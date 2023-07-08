using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMover : MonoBehaviour
{
    private Grid _worldGrid;

    // Start is called before the first frame update
    void Start()
    {
        _worldGrid = GameObject.FindGameObjectWithTag("WorldGrid").GetComponent<Grid>();
        transform.position = _worldGrid.CellToWorld(new Vector3Int(1, 0, 0)) + new Vector3(-0.5f, 0.5f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
