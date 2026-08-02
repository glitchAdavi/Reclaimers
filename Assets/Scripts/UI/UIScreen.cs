using Reclaimers.Systems;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private Image _image;

    public float turnDuration = 0.2f;
    public float turnStepDuration = 0.01f;
    public bool turnedOn = false;

    Timer turnTimer;

    private void Awake()
    {
        Material materialInstance = Instantiate(_image.material);
        _image.material = materialInstance;

        if (turnedOn) _image.material.SetFloat("_TurnOn", 1f);
    }

    public void TurnOn()
    {
        turnedOn = true;
        turnTimer?.Finish();
        turnTimer = GameManager.Instance.timerSystem.SetTimer(turnDuration, () =>
        {
            _image.material.SetFloat("_TurnOn", 1f);
        }, turnStepDuration, () =>
        {
            _image.material.SetFloat("_TurnOn", turnTimer.durationElapsed / turnDuration);
        }, "UI_Screen_TurnOn");
    }

    public void ForceOn()
    {
        _image.material.SetFloat("_TurnOn", 1f);
        turnedOn = true;
    }

    public void TurnOff()
    {
        turnedOn = false;
        turnTimer?.Finish();
        turnTimer = GameManager.Instance.timerSystem.SetTimer(turnDuration, () =>
        {
            _image.material.SetFloat("_TurnOn", 0f);
        }, turnStepDuration, () =>
        {
            _image.material.SetFloat("_TurnOn", 1 - turnTimer.durationElapsed / turnDuration);
        }, "UI_Screen_TurnOff");
    }

    public void ForceOff()
    {
        _image.material.SetFloat("_TurnOn", 0f);
        turnedOn = false;
    }

    public void SetScreenColor(Color c)
    {
        _image.material.SetColor("_TurnedOnColor", c);
    }
}
