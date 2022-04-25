
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Player[] playerPrefabs = new Player[4];

    private Player player;
    [SerializeField] HitDialog hitDialog;
    [SerializeField] ComboDialog comboDialog;

    private void Awake()
    {
        SetUp();
    }

    private void Start()
    {
        OnNewGame();
    }

    private void SetUp()
    {
        GameManager.LoadGame();
        GameMaster.Restart += OnNewGame;
        AudioManager.instance.Play("Background");
    }

    private void OnDestroy()
    {
        GameMaster.Restart -= OnNewGame;
    }

    public void OnNewGame()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }
        player = Instantiate(playerPrefabs[Utility.select], transform);
        player.transform.GetChild(0).GetComponent<Gun>().SetDialogs(hitDialog, comboDialog);
    }
}
