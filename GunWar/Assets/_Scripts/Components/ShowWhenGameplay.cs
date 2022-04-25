
using UnityEngine;

public class ShowWhenGameplay : MonoBehaviour
{
    private void Awake()
    {
        GameMaster.EnterMenu += Hide;
        GameMaster.GameOver += Hide;
        GameMaster.Restart += Show;
    }

    private void OnDestroy()
    {
        GameMaster.EnterMenu -= Hide;
        GameMaster.GameOver -= Hide;
        GameMaster.Restart -= Show;
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
}
