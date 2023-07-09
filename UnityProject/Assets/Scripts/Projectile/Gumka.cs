using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumka : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private Animator _animator;

    public Vector3 direction = Vector3.right;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("dirX", direction.x);
        _animator.SetFloat("dirY", direction.y);
        if (Vector3.Distance(transform.position, Vector3.zero) > 50.0f)
            Destroy(gameObject);
        transform.position += direction * _speed * Time.deltaTime;
    }
}
