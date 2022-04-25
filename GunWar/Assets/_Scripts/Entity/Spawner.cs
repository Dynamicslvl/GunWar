
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public FlyGround flyGround;
    private FlyGround[] fg = new FlyGround[2];

    private int id = 0;
    private float offset, minOffset = 0.5f, maxOffset = 2f;

    private void Awake()
    {
        GameMaster.Restart += Renew;
        GameMaster.EnterMenu += Init;
        SetUp();
    }

    private void OnDestroy()
    {
        GameMaster.Restart -= Renew;
        GameMaster.EnterMenu -= Init;
    }

    void SetUp()
    {
        offset = Random.Range(minOffset, maxOffset);
        fg[0] = Instantiate(flyGround, new Vector3(offset, 0, 0), Quaternion.identity);
        fg[1] = Instantiate(flyGround, new Vector3(7.5f, 0, 0), Quaternion.identity);
    }

    void Update()
    {
        if (fg[id].transform.position.x <= offset)
        {
            GameMaster.StopMove?.Invoke();
            offset = Random.Range(minOffset, maxOffset);
            id = 1 - id;
            fg[id].Renew(offset);
        }
    }

    void Renew()
    {
        offset = Random.Range(minOffset, maxOffset);
        fg[1 - id].Reposition(Random.Range(minOffset, maxOffset), false);
    }

    private void Init()
    {
        id = 1;
        offset = Random.Range(1, maxOffset);
        fg[0].Reposition(Random.Range(1, maxOffset), true);
        offset = Random.Range(minOffset, maxOffset);
        fg[1].Renew(Random.Range(minOffset, maxOffset));
    }
}
