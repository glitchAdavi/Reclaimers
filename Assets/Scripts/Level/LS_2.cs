using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_2 : LevelScript
{
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
        GameManager.current.eventService.SetPawnSpawnVars(1f, 10, 100);
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute1();
    }

    public override void Minute2()
    {
        GameManager.current.eventService.SetPawnSpawnVars(1f, 10, 100);
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute2();
    }

    public override void Minute3()
    {
        GameManager.current.eventService.SetPawnSpawnVars(1f, 10, 100);
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute3();
    }

    public override void Minute4()
    {
        GameManager.current.eventService.SetPawnSpawnVars(1f, 10, 100);
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute4();
    }

    public override void Minute5()
    {
        GameManager.current.eventService.SetPawnSpawnVars(1f, 10, 100);
        GameManager.current.eventService.PawnServiceClearSpawns();
        GameManager.current.eventService.PawnServiceAddSpawn(GameManager.current.gameInfo.slimeEnemyStatBlock, 100);

        base.Minute5();
    }
}
