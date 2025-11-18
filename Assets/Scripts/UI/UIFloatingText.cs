using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIFloatingText : MonoBehaviour, IUpdate, IPause
{
    [SerializeField] TMP_Text damageText;

    Timer deathTimer;

    public float speed = 2f;
    public float deathTimerTime = 0.5f;

    public float drift = 0f;

    private bool isPaused = false;

    private void OnEnable()
    {
        deathTimer = GameManager.current.timerService.StartTimer(deathTimerTime, Die);

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        damageText.color = Color.white;
    }

    public void ExecuteUpdate()
    {
        if (!isPaused)
        {
            transform.position += transform.up * speed * Time.deltaTime + transform.right * drift * Time.deltaTime;
        }
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
    }

    private void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    public void Init(string text, Color c, float driftRange, float duration)
    {
        drift = Random.Range(-driftRange, driftRange);
        damageText.color = c;
        damageText.text = text;
    }

    public void Die()
    {
        GameManager.current.uiService.floatingTextBuilder.ReturnObject(this);
    }

    public static void TurnOn(UIFloatingText dn)
    {
        dn.gameObject.SetActive(true);
    }

    public static void TurnOff(UIFloatingText dn)
    {
        dn.gameObject.SetActive(false);
    }
}
