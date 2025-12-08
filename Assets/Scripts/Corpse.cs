using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;

    [SerializeField] float decayRate = 0.01f;
    [SerializeField] float alpha;

    Timer timerDecay;

    public void Init(Sprite spr)
    {
        float angle = Random.Range(0f, 359f);
        transform.Rotate(gameObject.transform.up, angle);
        _sr.sprite = spr;
        _sr.enabled = true;
        timerDecay = GameManager.current.timerService.StartTimer(3600f, DestroyCorpse, 1f, Decay);
    }

    public void Decay()
    {
        Color color = _sr.color;
        color.a -= decayRate;
        alpha = color.a;
        _sr.color = color;
        if (color.a <= decayRate) DestroyCorpse();
    }

    public void DestroyCorpse()
    {
        if (timerDecay != null) timerDecay.Cancel();
        Destroy(gameObject);
    }
}
