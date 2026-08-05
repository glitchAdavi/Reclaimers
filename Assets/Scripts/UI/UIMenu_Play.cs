using System.Collections.Generic;
using Reclaimers.Systems;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu_Play : UIMenu
{
    public List<Toggle> levelButtons = new List<Toggle>();
    public List<Toggle> difficultyButtons = new List<Toggle>();

    public int nextLevelIndex = 1;
    public Difficulty selectedDifficulty = Difficulty.Normal;

    Timer startTimer;

    protected override void OnOpen()
    {
        base.OnOpen();

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out Toggle b);
            if (b != null) b.interactable = true;
        }

        levelButtons[0].isOn = true;
        difficultyButtons[0].isOn = true;
    }

    public override void BeforeClose()
    {
        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out Toggle b);
            if (b != null) b.interactable = false;
        }

        base.BeforeClose();
    }

    public void StartGame()
    {
        GameManager.Instance.dataPersistenceSystem.SaveGame();

        GameManager.Instance.selectedDifficulty = selectedDifficulty;
        startTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () =>
        {
            GameManager.Instance.uiSystem.FadeOut(() =>
            {
                GameManager.Instance.GoToLevel(nextLevelIndex);
            });
        });
    }

}
