using UnityEngine;

public interface IBuildable<T> where T : MonoBehaviour
{
    public static void TurnOn(T buildable)
    {
        buildable.gameObject.SetActive(true);
    }
    public static void TurnOff(T buildable)
    {
        buildable.gameObject.SetActive(false);
    }
}
