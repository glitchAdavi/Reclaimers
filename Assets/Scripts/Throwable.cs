using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;

    Vector3 start;
    Vector3 end;
    Timer timerTravel;


    public void Init(Vector3 start, Vector3 end, float time, Sprite spr)
    {
        _sr.sprite = spr;
        this.start = start;
        this.end = end;
        timerTravel = GameManager.current.timerService.StartTimer(time, Die, 0.02f, Travel, "", true);
    }

    public void Travel()
    {
        transform.position = Vector3.Lerp(start, end, timerTravel.lifeTime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
