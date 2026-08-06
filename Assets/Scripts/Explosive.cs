using Reclaimers.Systems;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] Light flash;
    [SerializeField] StatusEffect toApply;
    [SerializeField] GameObject aura;
    [SerializeField] SpriteRenderer sr;

    [SerializeField] float radius = 0f;
    [SerializeField] float damage = 0f;

    Timer delayTimer;
    Timer deathTimer;

    public void Init(float d, float r, float delay = 0f, StatusEffect toApply = null, Sprite sp = null)
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.grenadeLandAudio, transform.position, 0, -1);

        if (sp != null) sr.sprite = sp;
        radius = r;
        damage = d;
        this.toApply = toApply;
        aura.SetActive(true);
        aura.transform.localScale = new Vector3(radius, radius, radius);
        aura.GetComponent<AuraRotate>().SetRotateAngle(0.1f);
        if (delay == 0) Explode();
        else
        {
            delayTimer = GameManager.Instance.timerSystem.SetTimer(delay, Explode, "EXPLOSIVE_Delay");
        }
    }

    public void Explode()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.explosionAudio, transform.position, 0, -1);

        flash.enabled = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, 1 << 11);
        foreach (Collider c in hits)
        {
            Pawn e = c.gameObject.GetComponentInParent<Pawn>();
            e.GetHit(-damage, false, 0.35f);
            e.AddStatusEffect(toApply);
        }

        deathTimer = GameManager.Instance.timerSystem.SetTimer(0.05f, () => Destroy(gameObject), "EXPLOSIVE_Death");
    }

}
