
public static class Utility
{
    public static int score = 0, highScore = 0;
    public static int coin = 1000;
    public static int select = 0;
    public static int bought = 1; // An mask of bought characters: 1 ⟺ 0001 means P1 has bought
    public static int onSound, onMusic;
    public static int[] cost = {0, 50, 75, 100};
    public static bool haveNewBest = false;
    public static bool inGame = false;
}
