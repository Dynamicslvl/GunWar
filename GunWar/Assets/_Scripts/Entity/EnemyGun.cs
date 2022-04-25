using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public EnemyBullet bullet;
    public float aimSpeed = 50;
    public Vector2 target = new Vector2(-2, -2);
    private float aimAngle;
    private bool shot = false;

    private void OnEnable()
    {
        GameMaster.EnemyTurn += Shot;
        GameMaster.Restart += Renew;
        GameMaster.EnterMenu += Renew;
    }

    private void OnDisable()
    {
        GameMaster.EnemyTurn -= Shot;
        GameMaster.Restart -= Renew;
        GameMaster.EnterMenu -= Renew;
    }

    void Shot()
    {
        if (Mathf.Abs(transform.position.x) >= 3) return;
        shot = true;
        Vector2 pos = transform.position;
        Vector2 diff = target - pos;
        aimAngle = Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 360;
    }

    void Update()
    {
        if (!shot) return;
        if (transform.eulerAngles.z < aimAngle)
        {
            transform.Rotate(0, 0, -aimSpeed*Time.deltaTime);
        } else 
        {
            shot = false;
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, aimAngle));
        }
    }

    void Renew()
    {
        shot = false;
        transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
