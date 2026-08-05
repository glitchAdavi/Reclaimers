using Reclaimers.Systems;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] Light _light;
    bool scriptEnabled = false;

    Timer flickeringTimer;

    public void Enable(bool enable)
    {
        scriptEnabled = enable;
        _light.enabled = enable;

    }

}
