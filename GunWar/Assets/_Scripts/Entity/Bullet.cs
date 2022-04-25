using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{

    private float mainSpeed;
    private float gravityAcc;
    private Gun owner;
    private float vSpeed, hSpeed;
    private bool move = true;

    public Gun Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
            switch (owner.gunType)
            {
                case GunType.Bazooka:
                    mainSpeed = 9;
                    gravityAcc = 6;
                    break;
                case GunType.WaterGun:
                    mainSpeed = 7;
                    gravityAcc = 4;
                    break;
                case GunType.LaserGun:
                    mainSpeed = 50;
                    gravityAcc = 0;
                    break;
                case GunType.XmasGun:
                    mainSpeed = 8;
                    gravityAcc = 5;
                    break;
            }
        }
    }

    void Awake()
    {
        GameMaster.GameOver += SetNoMoreValue;
        GameMaster.MoveToNextTarget += SetNoMoreValue;
        GameMaster.EnterMenu += StopAllCoroutines;
        GameMaster.EnterMenu += DestroySelf;
    }

    void OnDestroy()
    {
        GameMaster.GameOver -= SetNoMoreValue;
        GameMaster.MoveToNextTarget -= SetNoMoreValue;
        GameMaster.EnterMenu -= StopAllCoroutines;
        GameMaster.EnterMenu -= DestroySelf;
    }

    void DestroySelf()
    {
        missCount = 0;
        Destroy(gameObject);
    }

    static int missCount = 0;
    int value = 1;

    void SetNoMoreValue()
    {
        value = 0;
        missCount = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    void Start()
    {
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        hSpeed = mainSpeed * Mathf.Cos(angle);
        vSpeed = mainSpeed * Mathf.Sin(angle);
    }

    void Update()
    {
        Movement();
        CheckOutside();
    }

    // LASER GUN ONLY
    private void FixedUpdate()
    {
        if (owner == null) return;
        if (owner.gunType != GunType.LaserGun || hit) return;

        float r = 1f/60;
        if (owner.Combo == 3)
        {
            r *= 5;
        }

        RaycastHit2D headshot = Physics2D.CircleCast(transform.position, r, transform.right, 6f, LayerMask.GetMask("EnemyHead"));
        RaycastHit2D bodyhit  = Physics2D.CircleCast(transform.position, r, transform.right, 6f, LayerMask.GetMask("EnemyBody"));
        
        if (headshot)
        {
            hit = true;
            owner.SetCombo(true, headshot.collider.transform.position);
            CollideWithEnemy(headshot.collider);
            Explosion(headshot.point);
        } else if (bodyhit)
        {
            hit = true;
            owner.SetCombo(false, bodyhit.collider.transform.position);
            CollideWithEnemy(bodyhit.collider);
            Explosion(bodyhit.point);
        } else
        {
            Miss();
        }
    }

    private bool fall = false;

    void Movement()
    {
        if (!move) return;
        float speed = Mathf.Sqrt(hSpeed * hSpeed + vSpeed * vSpeed);
        transform.Translate(speed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(vSpeed, hSpeed));
        vSpeed -= gravityAcc * Time.deltaTime;

        // XMAS GUN ONLY
        if (owner == null) return;
        if (owner.gunType == GunType.XmasGun)
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                fall = true;
            }
            if (fall)
            {
                hSpeed = Mathf.Max(0, hSpeed - 1.5f * gravityAcc * Time.deltaTime);
                vSpeed -= 2 * gravityAcc * Time.deltaTime;
            }
        }
    }

    void CheckOutside()
    {
        if (Mathf.Abs(transform.position.y) > 7f || Mathf.Abs(transform.position.x) > 4f)
        {
            if (owner == null) return;
            if (owner.gunType != GunType.LaserGun)
                Miss();
            else
                StartCoroutine(ActiveDestroySelf(1));
        }
    }

    void Miss()
    {
        if (owner == null) return;

        // Destroy no matter what
        StartCoroutine(ActiveDestroySelf(1f));
        GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<CapsuleCollider2D>().enabled = false;
        missCount += value;
        move = (owner.gunType == GunType.LaserGun);

        // If player is alive
        if (owner.Owner.gameObject.activeSelf)
        {
            if (owner.gunType != GunType.WaterGun || missCount == 3)
            {
                Debug.Log("Missed");
                missCount = 0;
                owner.Miss();
                GameMaster.EnemyTurn?.Invoke();
            }
        }

        owner = null;
    }

    private bool hit = false;

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (owner == null) return;
        if (owner.gunType == GunType.LaserGun) return;
        if (hit) return;
        if (collider.CompareTag("Player"))
        {
            hit = true;
            CollideWithPlayer(collider);
            Explosion(transform.position);
        } else if (collider.CompareTag("Obstacle") && owner.gunType != GunType.LaserGun)
        {
            hit = true;
            CollideWithObstacle(collider);
        }
        if (collider.CompareTag("EnemyHead"))
        {
            hit = true;
            owner.SetCombo(true, collider.transform.parent.position);
            CollideWithEnemy(collider);
            Explosion(transform.position);
        } else if (collider.CompareTag("EnemyBody"))
        {
            hit = true;
            owner.SetCombo(false, collider.transform.parent.position);
            CollideWithEnemy(collider);
            Explosion(transform.position);
        }
    }

    private void CollideWithPlayer(Collider2D collider)
    {
        Explosion(transform.position);
        PoolingSystem.instance.GetEffect(ParticleType.Blood, collider.transform.position);
        Vector2 F = new Vector2(-Random.Range(25, 50), Random.Range(200, 400));
        PoolingSystem.instance
            .GetDeadBody(collider.transform.position, Vector3.zero, F)
            .setSprite(collider.GetComponent<Player>().sprite);
        Debug.Log("You're dead!");
        GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<TrailRenderer>().enabled = false;
        move = (owner.gunType == GunType.LaserGun);
        collider.gameObject.SetActive(false);
        StartCoroutine(ActiveGameOver(1));
    }

    private void CollideWithObstacle(Collider2D collider)
    {
        Explosion(transform.position);
        Miss();
    }

    private void CollideWithEnemy(Collider2D collider)
    {
        AudioManager.instance.Play("Coin");
        move = (owner.gunType == GunType.LaserGun);
        GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(ActiveDestroySelf(1f));
        collider.transform.parent.gameObject.SetActive(false);
        Vector3 enemyPos = collider.transform.parent.position;
        Vector2 F = new Vector2(Random.Range(50, 100), Random.Range(200, 400));
        PoolingSystem.instance.GetDeadBody(enemyPos, Vector3.zero, F);
        PoolingSystem.instance.GetEffect(ParticleType.Blood, enemyPos);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void Explosion(Vector3 pos)
    {
        switch (owner.gunType)
        {
            case GunType.Bazooka:
                PoolingSystem.instance.GetEffect(ParticleType.Explosion, pos);
                AudioManager.instance.Play("Explosion1");
                break;
            case GunType.WaterGun:
                PoolingSystem.instance.GetEffect(ParticleType.WaterBlast, pos);
                AudioManager.instance.Play("Explosion2");
                break;
            case GunType.LaserGun:
                PoolingSystem.instance.GetEffect(ParticleType.LaserBlast, pos, transform.eulerAngles);
                AudioManager.instance.Play("Explosion3");
                break;
            case GunType.XmasGun:
                PoolingSystem.instance.GetEffect(ParticleType.DecorBlast, pos);
                AudioManager.instance.Play("Explosion4");
                break;
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

    IEnumerator ActiveDestroySelf(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}
