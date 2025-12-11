using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_3 : LevelScript
{
    float i = 1f; //interval
    int b = 10; //batch
    int m = 100; //max

    public override void StartStage1Script()
    {
        b = 10;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.StartStage1Script();
    }

    public override void StartStage2Script()
    {
        GameManager.current.eventService.PawnServiceClearSpawns();

        base.StartStage2Script();
    }

    public override void Minute1()
    {
        i = 3f;
        b = 10;
        m = 30;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);
        //GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.bugEnemyStatBlock, 100);
        //GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.zombieEnemyStatBlock, 100);

        base.Minute1();
    }

    public override void Minute2()
    {
        i = 3f;
        b = 10;
        m = 50;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 95);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.zombieEnemyStatBlock, 5);

        base.Minute2();
    }

    public override void Minute3()
    {
        i = 3f;
        b = 10;
        m = 75;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 80);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.zombieEnemyStatBlock, 20);

        base.Minute3();
    }

    public override void Minute4()
    {
        i = 3f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 75);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.zombieEnemyStatBlock, 20);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.bugEnemyStatBlock, 5);

        base.Minute4();
    }

    public override void Minute5()
    {
        i = 3f;
        b = 10;
        m = 150;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 60);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.zombieEnemyStatBlock, 20);
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.bugEnemyStatBlock, 20);

        base.Minute5();
    }
}
