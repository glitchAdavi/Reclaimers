using UnityEngine;
using Reclaimers.UpdateSystem;

namespace Reclaimers.Systems
{
    public class UISystem : MonoBehaviour, IPauseable
    {
        public ObjectBuilder<FloatingText> floatingTextBuilder;

        private void OnEnable()
        {
            this.Register();

            GameObject textParent = new GameObject();
            textParent.name = "FloatingTextParent";
            floatingTextBuilder = new ObjectBuilder<FloatingText>();
            floatingTextBuilder.BuildPool(FloatingTextFactory, 100, textParent.transform);

            EventSystem.Instance.onSpawnFloatingText += SpawnFloatingText;
        }

        public void ManagedPause(bool paused)
        {

        }





        public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float maxSize)
        {
            FloatingText newFT = floatingTextBuilder.GetObject();
            newFT.transform.position = new Vector3(pos.x, 1f, pos.z + 0.25f);
            newFT.Init(text, c, driftRange, maxSize);
        }

        public FloatingText FloatingTextFactory(Transform parent = null)
        {
            FloatingText ft = Instantiate(GameManager.Instance.gameInfo.floatingTextPrefab,
                                          Vector3.zero,
                                          GameManager.Instance.gameInfo.floatingTextPrefab.transform.rotation,
                                          parent)
                                          .GetComponent<FloatingText>();

            return ft;
        }

    }
}

