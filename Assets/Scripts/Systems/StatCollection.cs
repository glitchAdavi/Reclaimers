using UnityEngine;

public class StatCollection : MonoBehaviour
{
    [SerializeField] public int enemiesKilled = 0;
    [SerializeField] public float xpGained = 0;
    [SerializeField] public int levelsGained = 0;
    [SerializeField] public float moneyGained = 0;
    [SerializeField] public int upgradesGained = 0;
    [SerializeField] public int itemsGained = 0;

    private void OnEnable()
    {
        GameManager.Instance.eventSystem.onEnemyDeath += AddEnemyKilled;
        GameManager.Instance.eventSystem.onGivePlayerXp += AddXpGained;
        GameManager.Instance.eventSystem.onGivePlayerLevel += AddLevelGained;
        GameManager.Instance.eventSystem.onGivePlayerMoney += AddMoneyGained;
    }

    public void AddEnemyKilled(Pawn e)
    {
        enemiesKilled++;
        GameManager.Instance.eventSystem.UpdateKillCount(enemiesKilled);
    }

    public void AddXpGained(float gained)
    {
        xpGained += gained;
        GameManager.Instance.eventSystem.UpdateXpGained(xpGained);
    }

    public void AddLevelGained(int gained, bool t = false)
    {
        levelsGained += gained;
    }

    public void AddMoneyGained(float gained)
    {
        moneyGained += gained;
        GameManager.Instance.eventSystem.UpdateMoneyGained(moneyGained);
    }

    public void AddUpgradeGained()
    {
        upgradesGained++;
    }

    public void AddItemGained()
    {
        itemsGained++;
    }
}
