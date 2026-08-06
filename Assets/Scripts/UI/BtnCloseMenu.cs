using UnityEngine;

public class BtnCloseMenu : MonoBehaviour
{
    public void CloseMenu()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);
        GameManager.Instance.eventSystem.OpenCloseMenues();
    }
}
