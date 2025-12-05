using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_1 : LevelScript
{
    float i = 1f; //interval
    int b = 10; //batch
    int m = 100; //max

    public override void StartStage1Script()
    {
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
        i = 1f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute1();
    }

    public override void Minute2()
    {
        i = 1f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute2();
    }

    public override void Minute3()
    {
        i = 1f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute3();
    }

    public override void Minute4()
    {
        i = 1f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute4();
    }

    public override void Minute5()
    {
        i = 1f;
        b = 10;
        m = 100;
        scaling = GameManager.current.Scaling();
        GameManager.current.eventService.SetPawnSpawnVars(i, (int)(b * scaling), (int)(m * scaling));
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute5();
    }
}
