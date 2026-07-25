using UnityEngine;

public class ObstaclePart : MonoBehaviour
{
    [SerializeField] GameObject _front;
    [SerializeField] GameObject _top;

    public void SetActiveFront(bool active)
    {
        _front.gameObject.SetActive(active);
    }
}
