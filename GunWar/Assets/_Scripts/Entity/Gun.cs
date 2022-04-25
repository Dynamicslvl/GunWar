using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{

    private bool enableShot = true;
    private readonly float limitAngle = 90;
    private Animator ani;
    private float aimSpeed = 1;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Bullet superBullet;
    [SerializeField] private Player owner;
    private HitDialog hitDialog;
    private ComboDialog comboDialog;
    private ParticleSystem burn;

    public Sprite[] gunSpite = new Sprite[4];
    public GunType gunType;

    public Player Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public void SetDialogs(HitDialog hd, ComboDialog cd)
    {
        hitDialog = hd;
        comboDialog = cd;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        burn = transform.GetChild(0).GetComponent<ParticleSystem>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (gunType)
        {
            case GunType.Bazooka:
                aimSpeed = 1;
                sr.sprite = gunSpite[0];
                break;
            case GunType.WaterGun:
                aimSpeed = 0.9f;
                sr.sprite = gunSpite[1];
                break;
            case GunType.LaserGun:
                aimSpeed = 0.8f;
                sr.sprite = gunSpite[2];
                break;
            case GunType.XmasGun:
                aimSpeed = 0.85f;
                sr.sprite = gunSpite[3];
                break;
        }
    }

    private void Awake()
    {
        GameMaster.MoveToNextTarget += ResetAim;
        GameMaster.StopMove += EnableShot;
        GameMaster.Restart += Renew;
        GameMaster.EnterMenu += StopAllCoroutines;
        GameMaster.EnterMenu += Renew;
    }

    private void OnDestroy()
    {
        GameMaster.MoveToNextTarget -= ResetAim;
        GameMaster.StopMove -= EnableShot;
        GameMaster.Restart -= Renew;
        GameMaster.EnterMenu -= StopAllCoroutines;
        GameMaster.EnterMenu -= Renew;
    }

    private bool holding = false;

    void Update()
    {
        if (!Utility.inGame) return;
        if (!enableShot) return;
        // Start aim
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                holding = true;
            }
        }
        // Aiming
        if (holding)
        {
            transform.Rotate(0, 0, 50 * aimSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.z >= limitAngle)
            {
                transform.rotation = Quaternion.Euler(0, 0, limitAngle);
            }
        }
        // Shot
        if (Input.GetMouseButtonUp(0) && holding)
        {
            holding = false;
            enableShot = false;
            owner.GetComponent<BoxCollider2D>().enabled = false;
            switch (gunType)
            {
                case GunType.LaserGun:
                    StartCoroutine(ActivePlayerCollider(.1f));
                    AudioManager.instance.Play("LaserShot");
                    break;
                case GunType.WaterGun:
                    StartCoroutine(ActivePlayerCollider(.5f));
                    AudioManager.instance.Play("WaterShot");
                    break;
                case GunType.Bazooka:
                    StartCoroutine(ActivePlayerCollider(.5f));
                    AudioManager.instance.Play("FlyMissle");
                    break;
                case GunType.XmasGun:
                    StartCoroutine(ActivePlayerCollider(.5f));
                    AudioManager.instance.Play("FlyGift");
                    break;
            }
            if (combo < 3)
            {
                Instantiate(bullet, transform.position, transform.rotation).Owner = this;
                if (gunType == GunType.WaterGun)
                {
                    ShotSideBullets(8, bullet);
                }
            }
            else
            {
                burn.Stop();
                Instantiate(superBullet, transform.position, transform.rotation).Owner = this;
                if (gunType == GunType.WaterGun)
                {
                    ShotSideBullets(8, superBullet);
                }
            }
            ani.SetTrigger("Shot");
        }
    }

    void ShotSideBullets(float angle, Bullet bulletType)
    {
        Quaternion rot1 = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, angle));
        Quaternion rot2 = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, -angle));
        Instantiate(bulletType, transform.position, rot1).Owner = this;
        Instantiate(bulletType, transform.position, rot2).Owner = this;
    }

    IEnumerator ActivePlayerCollider(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        owner.GetComponent<BoxCollider2D>().enabled = true;
    }

    private int combo = 0;

    public int Combo
    {
        get
        {
            return combo;
        }
    }

    public void SetCombo(bool isHeadShot, Vector3 pos)
    {
        // Combo
        if (combo < 3 && isHeadShot)
        {
            StartCoroutine(ActiveMoveToNextTarget(0.5f));
            Camera.main.DOShakePosition(0.5f, 0.2f, 10, 5, true);
            int coin = Random.Range(5, 8);
            Utility.coin += coin;
            PoolingSystem.instance.GetEffect(ParticleType.CoinFalling, pos, default, coin);
            Utility.score += 2;
            combo++;
            Debug.Log("Headshot! " + combo);
            hitDialog.ShowHit(1);
            if (combo == 2)
            {
                var emission = burn.emission;
                emission.rateOverTime = 10;
                burn.Play();
            }
            if (combo == 3)
            {
                var emission = burn.emission;
                emission.rateOverTime = 30;
                Debug.Log("FEVER!");
                comboDialog.ShowCombo(0);
            }
        }
        else
        {
            var emission = burn.emission;
            emission.rateOverTime = 0;
            // No combo
            if (combo < 3)
            {
                StartCoroutine(ActiveMoveToNextTarget(0.5f));
                Camera.main.DOShakePosition(0.2f, 0.1f, 10, 5, true);
                int coin = Random.Range(2, 5);
                Utility.coin += coin;
                PoolingSystem.instance.GetEffect(ParticleType.CoinFalling, pos, default, coin);
                Utility.score++;
                hitDialog.ShowHit(0);
            } else
            // Ultra Kill
            {
                StartCoroutine(ActiveMoveToNextTarget(0.8f));
                Camera.main.DOShakePosition(1, 1, 10, 10, true);
                int coin = Random.Range(8, 11);
                Utility.coin += coin;
                PoolingSystem.instance.GetEffect(ParticleType.CoinFalling, pos, default, coin);
                Utility.score += 5;
                comboDialog.ShowCombo(1);
            }
            combo = 0;
            Debug.Log("Hit!");
        }
    }

    public void Miss()
    {
        hitDialog.ShowHit(2);
    }

    public void EnableShot()
    {
        enableShot = true;
    }

    public void ResetAim()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Renew()
    {
        Utility.score = 0;
        combo = 0;
        burn.Stop();
        EnableShot();
        ResetAim();
    }

    IEnumerator ActiveMoveToNextTarget(float waitTime)
    {
        while (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        GameMaster.MoveToNextTarget?.Invoke();
    }
}


public enum GunType
{
    Bazooka,
    WaterGun,
    LaserGun,
    XmasGun
}
