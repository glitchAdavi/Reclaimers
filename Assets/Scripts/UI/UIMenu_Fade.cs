using UnityEngine;
using Reclaimers.Systems;
using TMPro;

public class UIMenu_Fade : UIMenu
{
    [SerializeField] TMP_Text fadeMenuText;
    [SerializeField] TMP_Text fadeMenuStats;

    Timer returnTimer;

    public void SetText(string text)
    {
        fadeMenuText.text = text;
    }

    public void SetStats(string text)
    {
        fadeMenuStats.text = text;
    }

    public void ReturnToBase()
    {
        returnTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () =>
        {
            GameManager.Instance.GoToLevel(0);
        }, "UIMENU_Fade_ReturnToBase");
    }
}
