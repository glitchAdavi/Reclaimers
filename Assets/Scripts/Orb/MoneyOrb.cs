using UnityEngine;
using Reclaimers.Systems;
using System.Collections.Generic;

public class MoneyOrb : Orb
{
    [SerializeField] protected float value = 1;
    [SerializeField] protected List<Sprite> valueSprite = new List<Sprite>();

    public void Init(float v)
    {
        SetValue(v);
    }

    public void SetValue(float v)
    {
        value = v;

        _sr.sprite = valueSprite[Random.Range(0, valueSprite.Count)];

        // Change color depending value
    }

    protected override void Effect()
    {
        if (target != null)
        {
            if (value > 0) GameManager.Instance.eventSystem.GivePlayerMoney(value);
            GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.playerPickUpAudio, transform.position, 0, -1);
        }
    }

    public float GetValue()
    {
        return value;
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.eventSystem.MoneyOrbDeath(this);
    }

    protected override void Decay()
    {
        GameManager.Instance.eventSystem.MoneyOrbDecay(this);
        base.Decay();
    }

    protected override void UpdateVisibility()
    {
        base.UpdateVisibility();
        GameManager.Instance.eventSystem.MoneyOrbUpdateVisibility(this, isVisible);
    }
}
