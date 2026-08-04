using UnityEngine;
using UnityEngine.UI;
using Reclaimers.Systems;
using Reclaimers.DeveloperConsole;
using TMPro;

public class UIMenu_LevelUp : UIMenu
{
    public Button btnSkip;
    public TMP_Text pendingLevelUps;
    public UILevelUpButton u1;
    public UILevelUpButton u2;
    public UILevelUpButton u3;

    Timer closeTimer;

    protected override void OnOpen()
    {
        base.OnOpen();

        u1.Init(GameManager.Instance.GetRandomModifier());
        u2.Init(GameManager.Instance.GetRandomModifier());
        u3.Init(GameManager.Instance.GetRandomModifier());

        if (GameManager.Instance.playerPawn.HasPendingLevelUps(out int pending))
        {
            pendingLevelUps.enabled = true;
            pendingLevelUps.text = $"({pending} pending)";
        }
    }

    private void Update()
    {
        if (closeTimer == null && (u1.pressed || u2.pressed || u3.pressed)) BeforeClose();
    }

    public override void BeforeClose()
    {
        if (!u1.pressed) u1.Reset();
        if (!u2.pressed) u2.Reset();
        if (!u3.pressed) u3.Reset();

        closeTimer = GameManager.Instance.timerSystem.SetTimer(0.8f, Close);
    }

    protected override void OnClose()
    {
        base.OnClose();

        if (u1.pressed) u1.OnReset();
        if (u2.pressed) u2.OnReset();
        if (u3.pressed) u3.OnReset();
        pendingLevelUps.enabled = false;

        closeTimer = null;

        GameManager.Instance.eventSystem.LevelUpFinish();
    }
}
