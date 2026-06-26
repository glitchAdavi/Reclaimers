using System.Reflection;
using UnityEngine;

namespace Reclaimers.DataPersistence
{
    public class PlayerStatBlockData : MonoBehaviour, IDataPersistence
    {
        public void LoadData(GameData data)
        {
            foreach (FieldInfo fieldData in typeof(GameData).GetFields())
            {
                FieldInfo fieldStat = typeof(StatBlock).GetField(fieldData.Name);
                if (fieldStat != null)
                {
                    fieldStat.SetValue(GameManager.Instance.gameInfo.tempPlayerStatBlock, fieldData.GetValue(data));
                }
            }
        }

        public void SaveData(GameData data)
        {
            foreach (FieldInfo fieldStat in typeof(StatBlock).GetFields())
            {
                FieldInfo fieldData = typeof(GameData).GetField(fieldStat.Name);
                if (fieldData != null)
                {
                    fieldData.SetValue(data, fieldStat.GetValue(GameManager.Instance.playerPawn.statBlock));
                }
            }
        }
    }
}

