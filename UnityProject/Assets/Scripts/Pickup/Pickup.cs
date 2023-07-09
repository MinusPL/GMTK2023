using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PickupType
{
    Eraser,
    InkBomb,
    Sharpener,
    Brush
}

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupType _type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController p = collision.GetComponent<PlayerController>();
            switch(_type)
            {
                case PickupType.Eraser:
                    p.AddGumkaAmmo(1);
                    break;
                case PickupType.InkBomb:
                    p.AddInkShotAmmo(1);
                    break;
                case PickupType.Sharpener:
                    p.EnableSharpener();
                    break;
                case PickupType.Brush:
                    p.EnableBrush();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
