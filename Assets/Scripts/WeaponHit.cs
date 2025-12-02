using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Light _flash;

    Timer timerDeath;

    public void Activate(Sprite hitSprite, Color hitColor, float duration = 0.05f, float hitSize = 1)
    {
        transform.localScale = new Vector3(hitSize * transform.localScale.x, hitSize * transform.localScale.y, hitSize * transform.localScale.z);
        _sr.sprite = hitSprite;
        //_flash.color = hitColor;
        timerDeath = GameManager.current.timerService.StartTimer(duration, () => Destroy(gameObject));
    }
}
