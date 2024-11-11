using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager._instance.gamestate != GameStates.Paused)
            transform.Rotate(0, 0, speed);
    }
}
