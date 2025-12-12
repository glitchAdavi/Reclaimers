using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] Sprite disarmed;
    [SerializeField] Sprite armed;

    [SerializeField] SpriteRenderer _sr;

    float damage = 0f;
    float radius = 0f;
    float delay = 0f;
    Modifier mod;

    Timer timerArmMine;

    public bool isArmed = false;

    public void Init(float damage, float radius, float delay = 0f, Modifier mod = null)
    {
        this.damage = damage;
        this.radius = radius;
        this.delay = delay;
        this.mod = mod;
        timerArmMine = GameManager.current.timerService.StartTimer(0.5f, Arm);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.name == "DamageCollider")
        {
            if (isArmed) Explode();
        }
    }

    protected void Explode()
    {
        Instantiate(GameManager.current.gameInfo.explosionPrefab, transform.position, Quaternion.identity)
            .GetComponent<Explosive>().Init(damage, radius, delay, mod);
    }

    public void Arm()
    {
        isArmed = true;
        _sr.sprite = armed;
    }
}
