using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    private Rigidbody2D rg;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    public void setSprite(Sprite spr)
    {
        GetComponent<SpriteRenderer>().sprite = spr;
    }

    public void setForce(Vector2 F)
    {
        rg = GetComponent<Rigidbody2D>();
        rg.AddForce(F);
        rg.AddTorque(Random.Range(100f, 400f)*Mathf.Pow(-1, Random.Range(0, 2)));
    }

    private float scaleRate = 1.1f;

    private void Update()
    {
        float rate = transform.localScale.x*(1 - scaleRate);
        Vector3 scale = new Vector3(transform.localScale.x - rate * Time.deltaTime,
                                    transform.localScale.y - rate * Time.deltaTime,
                                    transform.localScale.z);
        transform.localScale = scale;
        if (transform.position.y < -5f)
        {
            gameObject.SetActive(false);
            rg.velocity = Vector2.zero;
        }
    }

}
