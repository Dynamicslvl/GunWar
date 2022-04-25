
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private BoxCollider2D headCollider, bodyCollider;

    private void Awake()
    {
        GameMaster.EnemyTurn += Aim;
        GameMaster.Restart += Stand;
        GameMaster.EnterMenu += Stand;
    }

    private void OnDestroy()
    {
        GameMaster.EnemyTurn -= Aim;
        GameMaster.Restart -= Stand;
        GameMaster.EnterMenu -= Stand;
    }

    public void Falling()
    {
        headCollider.gameObject.SetActive(false);
        bodyCollider.gameObject.SetActive(false);
    }

    public void Renew(Vector3 position, Vector3 rotation, Transform parent)
    {
        gameObject.SetActive(true);
        transform.parent = parent;
        transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        headCollider.gameObject.SetActive(true);
        bodyCollider.gameObject.SetActive(true);
    }

    void Aim()
    {
        ani.SetBool("isAim", true);
    }

    void Stand()
    {
        ani.SetBool("isAim", false);
    }
}
