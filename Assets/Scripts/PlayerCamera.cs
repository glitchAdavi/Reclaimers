using Reclaimers;
using Reclaimers.UpdateSystem;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, ILateUpdateable, IPauseable
{
    private Vector3Variable playerPosition;
    private Vector3Variable alternativePosition;
    public Transform transformToFollow;

    public bool followPlayer = true;

    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);
    public float smoothTime = 0.3f;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private bool isPaused = false;

    void Start()
    {
        playerPosition = GameManager.Instance.gameInfo.playerPositionVar;
        alternativePosition = GameManager.Instance.gameInfo.alternativePositionVar;
        transformToFollow = null;

        this.Register();
    }

    public void ManagedLateUpdate()
    {
        if (isPaused) return;

        if (followPlayer) transform.position = Vector3.SmoothDamp(transform.position, playerPosition.Value + cameraOffset, ref velocity, smoothTime);
        else
        {
            if (transformToFollow != null) transform.position = Vector3.SmoothDamp(transform.position, transformToFollow.position + cameraOffset, ref velocity, smoothTime);
            else transform.position = Vector3.SmoothDamp(transform.position, alternativePosition.Value + cameraOffset, ref velocity, smoothTime);
        }
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;
    }

    public void FollowPlayer(bool follow)
    {
        followPlayer = follow;
    }
}
