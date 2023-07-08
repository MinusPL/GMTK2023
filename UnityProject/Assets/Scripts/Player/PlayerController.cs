using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5.0f;

    private Vector2 _moveDirection;

    private Color _color = new() { r = 1.0f, g = 0.0f, b = 0.0f, a = 1.0f };

    //private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //_gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //basically deadzone
        if(_moveDirection.magnitude > 0.01f)
        {
            //Move to physics processing and base on RigidBody velocity
            transform.position += new Vector3(_moveDirection.normalized.x, _moveDirection.y, 0.0f) * _moveSpeed * Time.deltaTime;
        }
    }

    public void OnMove(InputValue value)
    {
        _moveDirection = value.Get<Vector2>();
    }

    public Color GetPlayerColor()
    {
        return _color;
    }
}
