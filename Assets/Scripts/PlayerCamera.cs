using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour, IUpdate, IFixedUpdate, ILateUpdate, IPause
{
    private Vector3Variable playerPosition;
    private Vector3Variable alternativePosition;

    public bool followPlayer = true;

    public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);
    public float smoothTime = 0.3f;

    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private bool isPaused = false;

    void Start()
    {
        playerPosition = GameManager.current.gameInfo.playerPositionVar;
        alternativePosition = GameManager.current.gameInfo.alternativePositionVar;
        GameManager.current.updateService.RegisterLateUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }

    public void ExecuteUpdate() { }

    public void ExecuteFixedUpdate() { }

    public void ExecuteLateUpdate()
    {
        if (isPaused) return;

        if (followPlayer) transform.position = Vector3.SmoothDamp(transform.position, playerPosition.Value + cameraOffset, ref velocity, smoothTime);
        else transform.position = Vector3.SmoothDamp(transform.position, alternativePosition.Value + cameraOffset, ref velocity, smoothTime);
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
    }

    public void FollowPlayer(bool follow)
    {
        followPlayer = follow;
    }
}
