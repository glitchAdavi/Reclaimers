using System.Collections.Generic;
using Reclaimers.Systems;
using UnityEngine;

public class XpOrb : Orb
{
    [SerializeField] protected float value = 1;
    [SerializeField] List<Sprite> valueSprites = new List<Sprite>();

    public void Init(float v)
    {
        SetValue(v);
    }

    public void SetValue(float v)
    {
        value = v;

        switch (value)
        {
            case > 0 and < 5:
                _sr.sprite = valueSprites[0];
                _sr.color = new Color(0, 1f, 1f, 1f);
                break;
            case >= 5 and < 10:
                _sr.sprite = valueSprites[0];
                _sr.color = Color.yellow;
                break;
            case >= 10 and < 25:
                _sr.sprite = valueSprites[0];
                _sr.color = Color.red;
                break;
            case >= 25 and < 50:
                _sr.sprite = valueSprites[1];
                _sr.color = new Color(0, 1f, 1f, 1f);
                break;
            case >= 50 and < 100:
                _sr.sprite = valueSprites[1];
                _sr.color = Color.yellow;
                break;
            case >= 100 and < 250:
                _sr.sprite = valueSprites[1];
                _sr.color = Color.red;
                break;
            case >= 250 and < 500:
                _sr.sprite = valueSprites[2];
                _sr.color = new Color(0, 1f, 1f, 1f);
                break;
            case >= 500 and < 1000:
                _sr.sprite = valueSprites[2];
                _sr.color = Color.yellow;
                break;
            case >= 1000:
                _sr.sprite = valueSprites[2];
                _sr.color = Color.red;
                break;
            default:
                break;
        }
    }

    protected override void Effect()
    {
        if (target != null)
        {
            if (value > 0) GameManager.Instance.eventSystem.GivePlayerXp(value);
        }
    }

    public float GetValue()
    {
        return value;
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.eventSystem.XpOrbDeath(this);
    }

    protected override void Decay()
    {
        GameManager.Instance.eventSystem.XpOrbDecay(this);
        base.Decay();
    }

    protected override void UpdateVisibility()
    {
        base.UpdateVisibility();
        GameManager.Instance.eventSystem.XpOrbUpdateVisibility(this, isVisible);
    }
}
