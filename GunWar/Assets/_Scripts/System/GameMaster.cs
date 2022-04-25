

public static class GameMaster
{
    public delegate void GameEvent();
    public static GameEvent GameOver;
    public static GameEvent Restart;
    public static GameEvent EnterMenu;

    public delegate void LevelEvent();
    public static LevelEvent EnemyTurn;
    public static LevelEvent MoveToNextTarget;
    public static LevelEvent StopMove;
}
