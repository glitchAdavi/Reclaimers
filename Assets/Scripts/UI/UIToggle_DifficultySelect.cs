using UnityEngine;

public class UIToggle_DifficultySelect : MonoBehaviour
{
    [SerializeField] UIMenu_Play menu;
    [SerializeField] Difficulty difficulty;

    public void SelectDifficulty()
    {
        menu.selectedDifficulty = difficulty;
    }
}
