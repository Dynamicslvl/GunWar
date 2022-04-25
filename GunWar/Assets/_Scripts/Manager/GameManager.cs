
using UnityEngine;

public static class GameManager
{
    public static void LoadGame()
    {
        Utility.highScore = PlayerPrefs.GetInt("HighScore");
        Utility.coin = PlayerPrefs.GetInt("Coin");
        Utility.select = PlayerPrefs.GetInt("Select");
        Utility.bought = Mathf.Max(1, PlayerPrefs.GetInt("Bought"));
        Utility.onMusic = PlayerPrefs.GetInt("OnMusic");
        Utility.onSound = PlayerPrefs.GetInt("OnSound");
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("HighScore", Utility.highScore);
        PlayerPrefs.SetInt("Coin", Utility.coin);
        PlayerPrefs.SetInt("Select", Utility.select);
        PlayerPrefs.SetInt("Bought", Utility.bought);
        PlayerPrefs.SetInt("OnSound", Utility.onSound);
        PlayerPrefs.SetInt("OnMusic", Utility.onMusic);
    }
}
