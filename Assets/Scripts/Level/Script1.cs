using UnityEngine;
using Reclaimers.Systems;

public class Script1 : LevelScript
{


    float i = 1f; //interval
    int b = 10; //batch
    int m = 100; //max



    public override void StartStage1Script()
    {
        b = 5;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.StartStage1Script();
    }

    public override void StartStage2Script()
    {
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();

        base.StartStage2Script();
    }

    public override void Minute1()
    {
        i = 3f;
        b = 10;
        m = 50;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute1();
    }

    public override void Minute2()
    {
        i = 3f;
        b = 10;
        m = 50;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute2();
    }

    public override void Minute3()
    {
        i = 3f;
        b = 10;
        m = 50;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute3();
    }

    public override void Minute4()
    {
        i = 3f;
        b = 10;
        m = 50;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute4();
    }

    public override void Minute5()
    {
        i = 3f;
        b = 10;
        m = 50;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute5();
    }
}
