using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;
using TMPro;
using Reclaimers.DeveloperConsole;

public class FloatingText : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] TMP_Text ftText;
    [SerializeField] SpriteRenderer ftImage;

    public float startPhaseDuration = 0.5f;
    public float middlePhaseDuration = 0.5f;
    public float endPhaseDuration = 0.2f;
    public float interval = 0.05f;

    public float speed = 2f;
    [SerializeField] private float targetSpeed = 0f;
    public float size = 0f;
    [SerializeField]private float targetSize = 1f;
    public float drift = 0f;
    [SerializeField] private float targetDrift = 0f;

    private bool isPaused = false;

    Timer timerReturn;

    public void Init(string text, Color c, Sprite image = null, float driftRange = 0f, float maxSize = 1f)
    {
        drift = Random.Range(-driftRange, driftRange);
        ftText.color = c;
        ftText.text = text;
        if (image != null)
        {
            ftImage.gameObject.SetActive(true);
            ftImage.sprite = image;
            ftImage.color = c;
        }
        transform.localScale = Vector3.one * size;

        speed += Random.Range(-0.3f, 0.3f);
        targetSize = maxSize;

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
        targetDrift = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(startPhaseDuration, MiddlePhase, interval, () =>
        {
            drift = Mathf.Lerp(drift, targetDrift, timerReturn.durationElapsed / startPhaseDuration);

            float currentSizeLerp = Mathf.Lerp(0f, targetSize, timerReturn.durationElapsed / endPhaseDuration);
            size = currentSizeLerp;
            transform.localScale = Vector3.one * size;
        });
    }

    private void MiddlePhase()
    {
        transform.localScale = Vector3.one * targetSize;
        speed = targetSpeed;
        drift = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(middlePhaseDuration, EndPhase);
    }

    private void EndPhase()
    {
        targetSize = 0f;

        timerReturn = GameManager.Instance.timerSystem.SetTimer(endPhaseDuration, Return, interval, () =>
        {
            float currentSizeLerp = Mathf.Lerp(size, targetSize, timerReturn.durationElapsed / endPhaseDuration);
            transform.localScale = Vector3.one * currentSizeLerp;
        });
    }

    private void Return()
    {
        timerReturn = null;
        drift = 0;
        speed = 2f;
        size = 0f;

        ftImage.gameObject.SetActive(false);

        GameManager.Instance.uiSystem.floatingTextBuilder.ReturnObject(this);
    }
}
