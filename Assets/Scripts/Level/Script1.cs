using UnityEngine;
using Reclaimers.Systems;

public class Script1 : LevelScript
{
    float i = 1f; //interval
    int b = 10; //batch
    int m = 100; //max

    public override void StartStage1Script()
    {
        i = 1f;
        b = 5;
        m = 200;

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
        m = 100;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 80);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.zombieEnemyStatBlock, 20);

        base.Minute1();
    }

    public override void Minute2()
    {
        i = 3f;
        b = 10;
        m = 120;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 20);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.zombieEnemyStatBlock, 80);

        base.Minute2();
    }

    public override void Minute3()
    {
        i = 1f;
        b = 25;
        m = 250;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.staticPlantEnemyStatBlock, 12);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.bugEnemyStatBlock, 85);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.mimicEnemyStatBlock, 3);

        base.Minute3();
    }

    public override void Minute4()
    {
        i = 2f;
        b = 20;
        m = 200;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.amalgamStatBlock, 80);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.zombieEnemyStatBlock, 17);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.mimicEnemyStatBlock, 3);

        base.Minute4();
    }

    public override void Minute5()
    {
        i = 2f;
        b = 30;
        m = 300;

        GameManager.Instance.eventSystem.SetPawnSpawnVars(i, b, m);
        GameManager.Instance.eventSystem.RequestClearPawnSpawns();
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.slimeEnemyStatBlock, 20);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.zombieEnemyStatBlock, 20);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.amalgamStatBlock, 20);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.staticPlantEnemyStatBlock, 7);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.bugEnemyStatBlock, 15);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.ratEnemyStatBlock, 15);
        GameManager.Instance.eventSystem.RequestAddPawnSpawn(GameManager.Instance.gameInfo.mimicEnemyStatBlock, 3);

        base.Minute5();
    }
}
