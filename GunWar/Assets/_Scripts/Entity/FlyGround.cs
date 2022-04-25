using DG.Tweening;
using UnityEngine;

public class FlyGround : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private Enemy enemyPrefab;
    private Enemy enemy;
    private bool moving = false;
    private readonly float minDis = 7;
    private readonly float minH =  -2;
    private readonly float maxH =   2;

    private void Awake()
    {
        GameMaster.MoveToNextTarget += Move;
        GameMaster.StopMove     += StopMove;

        // Must be init here
        float high = Random.Range(minH, 0.5f);
        transform.position = new Vector3(transform.position.x, high, 0);
        enemy = Instantiate(enemyPrefab, transform.GetChild(0));
        enemy.transform.SetPositionAndRotation(transform.GetChild(0).position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    private void OnDestroy()
    {
        GameMaster.MoveToNextTarget -= Move;
        GameMaster.StopMove     -= StopMove;
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
        transform.Translate(-movingSpeed * Time.deltaTime, 0, 0);
    }

    public void Renew(float offset)
    {
        float high = Random.Range(minH, maxH);
        transform.position = new Vector3(3f - offset + minDis, high, 0);
        enemy.gameObject.SetActive(true);
    }

    public void Reposition(float offset, bool isInit)
    {
        float high = Random.Range(minH, maxH);
        if (isInit)
        {
            high = Random.Range(minH, 0.5f);
        }
        enemy.gameObject.SetActive(true);
        transform.DOMove(new Vector3(offset, high, 0), 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
    }
}
