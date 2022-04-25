using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectacle : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float limit = -5.98f;
    private bool moving = false;

    private void OnEnable()
    {
        GameMaster.MoveToNextTarget += Move;
        GameMaster.StopMove += StopMove;
    }

    private void OnDestroy()
    {
        GameMaster.MoveToNextTarget -= Move;
        GameMaster.StopMove -= StopMove;
    }

    private void Move()
    {
        moving = true;
    }

    private void StopMove()
    {
        moving = false;
    }

    void Update()
    {
        if (!moving) return;
        transform.Translate(-movingSpeed*Time.deltaTime, 0, 0);
        if (transform.position.x <= limit)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}
