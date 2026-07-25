using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool editable = false;

    public bool horizontal = true;
    [Min(1)]
    public int partQuantity = 1;
    [SerializeField] GameObject part;
    [SerializeField] List<GameObject> parts = new List<GameObject>();
    [SerializeField] private Vector3 m_gridSize = new Vector3(0.5f, 0.7f, 0.7f);

    void Update()
    {
#if UNITY_EDITOR
        if (editable)
        {
            while (partQuantity > parts.Count)
            {
                parts.Add(Instantiate(part, transform));
            }

            while (partQuantity < parts.Count)
            {
                GameObject toDestroy = parts[^1];
                parts.RemoveAt(parts.Count - 1);
                DestroyImmediate(toDestroy);
            }

            var pos = transform.position;
            transform.position = new Vector3(
                Mathf.RoundToInt(pos.x / m_gridSize.x) * m_gridSize.x,
                Mathf.RoundToInt(pos.y / m_gridSize.y) * m_gridSize.y,
                Mathf.RoundToInt(pos.z / m_gridSize.z) * m_gridSize.z);


            float scaleCummulative = 0f;
            foreach (GameObject p in parts)
            {
                if (horizontal)
                {
                    p.transform.localPosition = new Vector3(((p.transform.localScale.x - 1) / 2) + scaleCummulative, 0f, 0f);
                    scaleCummulative += p.transform.localScale.x;
                    p.GetComponent<ObstaclePart>().SetActiveFront(true);
                }
                else
                {
                    p.transform.localPosition = new Vector3(0f, 0f, ((p.transform.localScale.z - 1.4f) / 2) + scaleCummulative);
                    scaleCummulative += p.transform.localScale.z;
                    if (!p.Equals(parts[0])) p.GetComponent<ObstaclePart>().SetActiveFront(false);
                }
            }
        } else
        {
            partQuantity = parts.Count;
        }
#endif
    }



}
