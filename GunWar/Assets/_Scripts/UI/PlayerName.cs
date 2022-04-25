
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    private TextMeshProUGUI playerName;

    private void Start()
    {
        playerName = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        switch (Utility.select)
        {
            case 0:
                playerName.text = "BAZOOKA BOY";
                break;
            case 1:
                playerName.text = "POWATER GIRL";
                break;
            case 2:
                playerName.text = "LIKELY ROBOT";
                break;
            case 3:
                playerName.text = "SANTA CLAUS";
                break;
        }
    }
}
