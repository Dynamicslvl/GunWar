using System;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    public static PoolingSystem instance;

    #region SINGLETON
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        } else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public DeadBody deadBody;
    private DeadBody[] DEAD;
    public ParticleSystem[] PART_PREFAB = new ParticleSystem[Enum.GetNames(typeof(ParticleType)).Length];
    private ParticleSystem[,] PART_POOL;
    private int[] id;

    int idb = 0;
    int maxPoolSize = 2;

    private void Start()
    {
        DEAD = new DeadBody[maxPoolSize];
        id = new int[Enum.GetNames(typeof(ParticleType)).Length];
        PART_POOL = new ParticleSystem[Enum.GetNames(typeof(ParticleType)).Length, maxPoolSize];
    }

    public ParticleSystem GetEffect(ParticleType type, Vector3 pos, Vector3 rot = default, int burstCount = default)
    {
        int i = (int) type;
        id[i] = (id[i] + 1) % maxPoolSize;
        if (PART_POOL[i, id[i]] == null)
        {
            PART_POOL[i, id[i]] = Instantiate(PART_PREFAB[i], transform);
        }
        PART_POOL[i, id[i]].gameObject.SetActive(true);
        PART_POOL[i, id[i]].transform.position = pos;
        PART_POOL[i, id[i]].transform.rotation = Quaternion.identity;
        if (rot != default)
        {
            PART_POOL[i, id[i]].transform.rotation = Quaternion.Euler(rot);
        }
        if (burstCount != default)
        {
            var burst = PART_POOL[i, id[i]].emission.GetBurst(0);
            burst.count = burstCount;
            PART_POOL[i, id[i]].emission.SetBurst(0, burst);
        }
        PART_POOL[i, id[i]].Play();
        return PART_POOL[i, id[i]];
    }

    public DeadBody GetDeadBody(Vector3 pos, Vector3 rot, Vector2 vel)
    {
        idb = (idb + 1) % maxPoolSize;
        if (DEAD[idb] == null)
        {
            DEAD[idb] = Instantiate(deadBody, transform);
        }
        DEAD[idb].gameObject.SetActive(true);
        DEAD[idb].transform.localScale = Vector3.one;
        DEAD[idb].transform.position = pos;
        DEAD[idb].transform.rotation = Quaternion.Euler(rot);
        DEAD[idb].setForce(vel);
        DEAD[idb].setSprite(deadBody.GetComponent<SpriteRenderer>().sprite);
        return DEAD[idb];
    }

    public void ResetPool()
    {
        foreach (ParticleSystem part in PART_POOL)
        {
            if (part != null)
            {
                part.gameObject.SetActive(false);
            }
        }
        foreach (DeadBody d in DEAD)
        {
            if (d != null)
            {
                d.gameObject.SetActive(false);
            }
        }
    }
}

public enum ParticleType
{
    Blood,
    Explosion,
    CoinFalling,
    WaterBlast,
    LaserBlast,
    DecorBlast
}