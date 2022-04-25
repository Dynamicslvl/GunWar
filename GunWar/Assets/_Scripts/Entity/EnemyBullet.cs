using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;

    private void OnEnable()
    {
        GameMaster.EnterMenu += StopAllCoroutines;
    }

    private void OnDestroy()
    {
        GameMaster.EnterMenu -= StopAllCoroutines;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            AudioManager.instance.Play("Explosion0");
            Camera.main.DOShakePosition(0.5f, 0.2f, 10, 5, true);
            PoolingSystem.instance.GetEffect(ParticleType.Blood, collider.transform.position);
            PoolingSystem.instance.GetEffect(ParticleType.Explosion, transform.position);
            Vector2 F = new Vector2(-Random.Range(25, 50), Random.Range(200, 400));
            PoolingSystem.instance.GetDeadBody(collider.transform.position, Vector3.zero, F)
                .setSprite(collider.GetComponent<Player>().sprite);
            Debug.Log("You're dead!");
            GetComponent<SpriteRenderer>().sprite = null;
            collider.gameObject.SetActive(false);
            StartCoroutine(ActiveGameOver(1));
        }
    }

    IEnumerator ActiveGameOver(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        GameMaster.GameOver?.Invoke();
        Destroy(gameObject);
    }
}
