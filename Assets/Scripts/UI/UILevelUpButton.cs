using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUpButton : MonoBehaviour
{
    [SerializeField] Modifier _mod;
    [SerializeField] Image _image;
    [SerializeField] TMP_Text _name;
    [SerializeField] TMP_Text _desc;
    [SerializeField] UIScreen _screen;

    Timer revealTimer;

    public bool pressed = false;

    public void Init(Modifier mod)
    {
        _mod = mod;

        if (mod.modImage != null)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = mod.modImage;
        }
        else _image.gameObject.SetActive(false);

        _name.text = mod.modName;
        _desc.text = mod.GetDesc();

        _image.color = SetColorAlpha(_image.color, 0f);
        _name.color = SetColorAlpha(_name.color, 0f);
        _desc.color = SetColorAlpha(_desc.color, 0f);

        _screen.SetScreenColor(GameManager.Instance.GetQualityColor(mod.quality));

        revealTimer = GameManager.Instance.timerSystem.SetTimer(_screen.turnDuration, OnReveal, _screen.turnStepDuration, () =>
        {
            _image.color = SetColorAlpha(_image.color, _image.color.a + revealTimer.durationElapsed / _screen.turnDuration);
            _name.color = SetColorAlpha(_name.color, _name.color.a + revealTimer.durationElapsed / _screen.turnDuration);
            _desc.color = SetColorAlpha(_desc.color, _desc.color.a + revealTimer.durationElapsed / _screen.turnDuration);
        });
        _screen.TurnOn();
    }

    public void OnReveal()
    {
        _image.color = SetColorAlpha(_image.color, 1f);
        _name.color = SetColorAlpha(_name.color, 1f);
        _desc.color = SetColorAlpha(_desc.color, 1f);
    }

    public void OnPress()
    {
        GameManager.Instance.playerPawn.AddModifier(_mod);
        pressed = true;
    }

    public void Reset()
    {
        revealTimer?.Finish();

        revealTimer = GameManager.Instance.timerSystem.SetTimer(_screen.turnDuration, OnReset, _screen.turnStepDuration, () =>
        {
            _image.color = SetColorAlpha(_image.color, _image.color.a - revealTimer.durationElapsed / _screen.turnDuration);
            _name.color = SetColorAlpha(_name.color, _name.color.a - revealTimer.durationElapsed / _screen.turnDuration);
            _desc.color = SetColorAlpha(_desc.color, _desc.color.a - revealTimer.durationElapsed / _screen.turnDuration);
        });

        _screen.TurnOff();
    }

    public void OnReset()
    {
        pressed = false;
        _mod = null;
        _image.color = SetColorAlpha(_image.color, 0f);
        _name.color = SetColorAlpha(_name.color, 0f);
        _desc.color = SetColorAlpha(_desc.color, 0f);
        _screen.ForceOff();
    }

    private Color SetColorAlpha(Color c, float val)
    {
        c.a = val;
        return c;
    }
}
