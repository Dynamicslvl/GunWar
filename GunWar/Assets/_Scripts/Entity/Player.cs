
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite sprite;
    public Animator ani;

    private void Awake()
    {
        GameMaster.Restart += Renew;
        GameMaster.EnterMenu += Renew;
        GameMaster.MoveToNextTarget += Run;
        GameMaster.StopMove += Stand;
    }

    private void OnDestroy()
    {
        GameMaster.Restart -= Renew;
        GameMaster.EnterMenu -= Renew;
        GameMaster.MoveToNextTarget -= Run;
        GameMaster.StopMove -= Stand;
    }

    void Renew()
    {
        gameObject.SetActive(true);
    }

    void Run()
    {
        ani.SetBool("isRunning", true);
    }

    void Stand()
    {
        ani.SetBool("isRunning", false);
    }
}
