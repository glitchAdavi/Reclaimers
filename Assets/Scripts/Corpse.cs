using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;

    [SerializeField] float decayRate = 0.03f;
    [SerializeField] float alpha;

    Timer timerDecay;

    public void Init(Sprite spr, float scale, int hueOffset)
    {
        float angle = Random.Range(0f, 359f);
        _sr.transform.localScale = new Vector3(scale, scale, scale);
        //_sr.transform.Rotate(gameObject.transform.up, angle);
        _sr.transform.rotation = Quaternion.Euler(new Vector3(90f, angle, _sr.transform.rotation.eulerAngles.z));
        _sr.sprite = spr;
        _sr.material.SetFloat("_HueOffset", hueOffset);
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

    public void ForceDecay()
    {
        decayRate = 0.25f;
    }

    public void DestroyCorpse()
    {
        if (timerDecay != null) timerDecay.Cancel();
        GameManager.current.pawnService.DeleteCorpse(this);
        Destroy(gameObject);
    }
}
