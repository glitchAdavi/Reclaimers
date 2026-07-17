using UnityEngine;

public class UIToggle_LevelSelect : MonoBehaviour
{
    [SerializeField] UIMenu_Play menu;
    [SerializeField] int levelIndex;

    public void SelectNextLevel()
    {
        menu.nextLevelIndex = levelIndex;
    }
}
