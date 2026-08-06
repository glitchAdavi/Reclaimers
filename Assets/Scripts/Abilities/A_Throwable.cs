using UnityEngine;

public class A_Throwable : Ability
{
    [SerializeField] Sprite throwableSprite;
    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    protected override void Effect()
    {
        Ray ray = GameManager.Instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (aimPlane.Raycast(ray, out float enterDistance))
        {
            Vector3 pos = ray.GetPoint(enterDistance);
            Instantiate(_p.statBlock.goToInstantiate, new Vector3(pos.x, 0.36f, pos.z), _p.statBlock.goToInstantiate.transform.rotation).
                GetComponent<Explosive>().
                Init(_p.statBlock.abilityDamage.Value(), _p.statBlock.abilityRadius.Value(), _p.statBlock.abilityDuration.Value(), _p.statBlock.seToApply, throwableSprite);
        }

    }
}
