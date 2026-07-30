using Reclaimers.DeveloperConsole;
using UnityEngine;

public class I_Container : Interactable
{
    public float lootSpreadRange = 5f;

    public Sprite opened;
    public Sprite closed;

    public Vector2Int xpMinMax = new Vector2Int(5, 10);
    public Vector2Int moneyMinMax = new Vector2Int(0, 5);
    public Vector2Int upgradeMinMax = new Vector2Int(0, 3);

    private void Awake()
    {
        useThreshold = 0.5f;
        _sr.sprite = closed;

        IText = $"{IVerb} {IName}";
    }

    protected override void Effect()
    {
        _sr.sprite = opened;

        int xpRange = Random.Range(xpMinMax.x, xpMinMax.y + 1);
        for (int i = 0; i < xpRange; i++)
        {
            LogSystem.Log($"({i})XP");
            GameManager.Instance.eventSystem.SpawnXp(GameManager.Instance.gridSystem.GetRandomPosInRadius(transform.position, lootSpreadRange + transform.localScale.x), 1f);
        }

        int materialRange = Random.Range(moneyMinMax.x, moneyMinMax.y + 1);
        for (int i = 0; i < materialRange; i++)
        {
            LogSystem.Log($"({i})Mat");
            GameManager.Instance.eventSystem.SpawnMoney(GameManager.Instance.gridSystem.GetRandomPosInRadius(transform.position, lootSpreadRange + transform.localScale.x), 1f);
        }

        int upgradeRange = Random.Range(upgradeMinMax.x, upgradeMinMax.y + 1);
        for (int i = 0; i < upgradeRange; i++)
        {
            LogSystem.Log($"({i})Upgrade");
            GameManager.Instance.eventSystem.SpawnRandomUpgrade(GameManager.Instance.gridSystem.GetRandomPosInRadius(transform.position, lootSpreadRange + transform.localScale.x));
        }
    }

    public override void Reset()
    {
        base.Reset();
        _sr.sprite = closed;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x + lootSpreadRange,
                                                            0.25f,
                                                            transform.localScale.z + lootSpreadRange));
    }
}
