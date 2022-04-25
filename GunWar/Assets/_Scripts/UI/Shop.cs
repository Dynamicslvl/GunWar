
using UnityEngine;

public class Shop : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.SaveGame();
    }
}
