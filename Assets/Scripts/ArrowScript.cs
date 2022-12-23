using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class ArrowScript : MonoBehaviour
{
    public float speed = 1f;
    public float endCoordinate = -9f;
    // Start is called before the first frame update
    void Start()
    {
            transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= endCoordinate)
        {
            Destroy(gameObject);
        }

        //moving
        transform.Translate(Vector2.right*speed);
    }

    public int getDamage()
    {
        return new Random().Next(1, 6);
    }

}
