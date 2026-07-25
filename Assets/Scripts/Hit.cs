using UnityEngine;
using Reclaimers.Systems;

public class Hit : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Light _flash;
    [SerializeField] Vector3 baseScale;

    Timer timerDeath;

    public void Activate(Sprite hitSprite, float hitSize = 1f)
    {
        transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        transform.localScale = baseScale;
        float randSize = Random.Range(0.95f, 1.05f);
        hitSize *= randSize;
        transform.localScale = new Vector3(hitSize * transform.localScale.x, hitSize * transform.localScale.y, hitSize * transform.localScale.z);
        _sr.sprite = hitSprite;
        timerDeath = GameManager.Instance.timerSystem.SetTimer(0.05f, () => Destroy(gameObject));
    }
}
