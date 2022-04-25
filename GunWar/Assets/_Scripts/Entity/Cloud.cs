using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed;
    private float limit;

    private void Start()
    {
        limit = -3 - GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
    }

    private void Update()
    {
        transform.Translate(-speed*Time.deltaTime, 0, 0);
        if (transform.position.x < limit)
        {
            speed = Random.Range(0.2f, 0.4f);
            float high = Random.Range(2f, 4f);
            transform.position = new Vector3(-limit, high, 0);
        }
    }
}
