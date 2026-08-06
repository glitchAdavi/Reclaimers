using UnityEngine;

public class UIToggle_LevelSelect : MonoBehaviour
{
    [SerializeField] UIMenu_Play menu;
    [SerializeField] int levelIndex;

    public void SelectNextLevel()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        menu.nextLevelIndex = levelIndex;
    }
}
