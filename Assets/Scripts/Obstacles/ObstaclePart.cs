using UnityEngine;

public class ObstaclePart : MonoBehaviour
{
    [SerializeField] GameObject _front;
    [SerializeField] GameObject _top;
    [SerializeField] bool horizontal = true;

    public void SetActiveFront(bool active)
    {
        _front.gameObject.SetActive(active);
    }

    public void SetHorizontal(bool active)
    {
        horizontal = active;
        if (horizontal) _top.transform.localScale += new Vector3(0f, 0.01f, 0f);
        else _top.transform.localScale -= new Vector3(0f, 0.01f, 0f);
    }
}
