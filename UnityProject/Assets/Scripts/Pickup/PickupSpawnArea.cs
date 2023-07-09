using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawnArea : MonoBehaviour
{
    [SerializeField]
    private Vector2 _size;
    // Start is called before the first frame update
    public Vector3 GetPoint()
    {
        return new Vector3(Random.Range(transform.position.x - (_size.x / 2), transform.position.x + (_size.x / 2)),
                            Random.Range(transform.position.y - (_size.y / 2), transform.position.y + (_size.y / 2)), 0);
    }

    private void OnDrawGizmos()
    {
        if(_size.magnitude > 0.0f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(_size.x, _size.y, 0f));
        }
    }
}
