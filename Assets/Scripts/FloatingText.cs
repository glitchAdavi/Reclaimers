using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;
using TMPro;
using Reclaimers.DeveloperConsole;

public class FloatingText : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] TMP_Text floatingText;



    public float phaseDuration = 1f;
    public float interval = 0.05f;

    public float speed = 2f;
    [SerializeField] private float targetSpeed = 0f;
    public float size = 6f;
    [SerializeField]private float targetSize = 0f;
    public float drift = 0f;
    [SerializeField] private float targetDrift = 0f;

    private bool isPaused = false;

    Timer timerReturn;

    public void Init(string text, Color c, float driftRange = 0f)
    {
        drift = Random.Range(-driftRange, driftRange);
        floatingText.color = c;
        floatingText.text = text;
        floatingText.fontSize = 0;

        if (timerReturn != null) timerReturn.Cancel();
        StartPhase();
    }

    private void OnEnable()
    {
        this.Register();
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        transform.position += transform.up * speed * Time.deltaTime + transform.right * drift * Time.deltaTime;
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;
    }

    private void OnDisable()
    {
        this.Unregister();
    }

    private void StartPhase()
    {
        targetSpeed = Random.Range(0.1f, 0.5f);
        targetSize = size;
        targetDrift = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(phaseDuration, MiddlePhase, interval, () =>
        {
            drift = Mathf.Lerp(drift, targetDrift, timerReturn.durationElapsed / phaseDuration);
            floatingText.fontSize = Mathf.Lerp(0f, targetSize, timerReturn.durationElapsed / phaseDuration);
        });
    }

    private void MiddlePhase()
    {
        floatingText.fontSize = size;
        speed = targetSpeed;
        drift = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(phaseDuration, EndPhase);
    }

    private void EndPhase()
    {
        targetSize = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(phaseDuration, Return, interval, () =>
        {
            floatingText.fontSize = Mathf.Lerp(size, targetSize, timerReturn.durationElapsed / phaseDuration);
        });
    }

    private void Return()
    {
        timerReturn = null;
        drift = 0;
        speed = 2f;

        floatingText.fontSize = 0;

        GameManager.Instance.uiSystem.floatingTextBuilder.ReturnObject(this);
    }

    public static void TurnOn(FloatingText ft)
    {
        ft.gameObject.SetActive(true);
    }

    public static void TurnOff(FloatingText ft)
    {
        ft.gameObject.SetActive(false);
    }
}
