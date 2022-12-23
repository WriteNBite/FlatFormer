using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{

    public GameObject arrow;
    public Vector3 spawnPoint;

    public int timerTime = 60;

    private int _timer;
    // Start is called before the first frame update
    void Start()
    {
        _timer = timerTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_timer <= 0)
        {
            _timer = timerTime;
            Instantiate(arrow, spawnPoint, Quaternion.identity);
        }
        else
        {
            _timer--;
        }
    }
}
