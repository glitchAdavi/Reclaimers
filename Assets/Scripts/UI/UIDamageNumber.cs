using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageNumber : MonoBehaviour, IUpdate, IPause
{
    [SerializeField] TMP_Text damageText;

    Timer deathTimer;

    public float speed = 2f;
    public float deathTimerTime = 0.5f;

    private bool isPaused = false;

    private void OnEnable()
    {
        deathTimer = GameManager.current.timerService.StartTimer(deathTimerTime, Die);

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }

    public void ExecuteUpdate()
    {
        if (!isPaused)
        {
            transform.position += transform.up * speed * Time.deltaTime;
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

    public void SetText(float damage, bool isCrit)
    {
        damageText.text = "";
        if (isCrit) damageText.text += "Crit!\n";
        damageText.text += $"{damage}";
    }

    public void Die()
    {
        GameManager.current.uiService.damageNumberBuilder.ReturnObject(this);
    }

    public static void TurnOn(UIDamageNumber dn)
    {
        dn.gameObject.SetActive(true);
    }

    public static void TurnOff(UIDamageNumber dn)
    {
        dn.gameObject.SetActive(false);
    }
}
