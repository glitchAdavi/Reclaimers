using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdate, IFixedUpdate, ILateUpdate, IPause
{
    [SerializeField] PlayablePawn playerPawn;

    [SerializeField] bool canControl = true;
    [SerializeField] bool isPaused = false;

    private void Awake()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }

    public void ExecuteUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ControllerEscape();

        if (!canControl || isPaused) return;

        ControllerMove();


        if (playerPawn.equippedWeapon.automatic)
        {
            if (Input.GetKey(KeyCode.Mouse0)) ControllerFire();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) ControllerFire();
        }

        if (Input.GetKeyDown(KeyCode.R)) ControllerReload();

        if (Input.GetKey(KeyCode.F)) ControllerInteract();
        else ControllerInteractReset();

    }

    public void ExecuteFixedUpdate()
    {
        if (!canControl || isPaused) return;

    }

    public void ExecuteLateUpdate()
    {
        if (!canControl || isPaused) return;

    }

    public void Pause(bool paused)
    {
        isPaused = paused;
    }

    public void AssignPlayerPawn(PlayablePawn newPawn)
    {
        playerPawn = newPawn;
    }

    #region Controls
    public void ControllerMove()
    {
        if (playerPawn == null) return;

        playerPawn.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        GameManager.current.gameInfo.playerPositionVar.SetValue(playerPawn.GetPosition());
    }

    public void ControllerFire()
    {
        if (playerPawn == null || playerPawn.equippedWeapon == null) return;

        playerPawn.equippedWeapon.Shoot();
    }

    public void ControllerReload()
    {
        if (playerPawn == null || playerPawn.equippedWeapon == null) return;

        GameManager.current.eventService.RequestUISpawnFloatingText(playerPawn.transform.position,
                                                                    "Reloading!",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);

        playerPawn.equippedWeapon.Reload();
    }

    public void ControllerEscape()
    {
        GameManager.current.updateService.TogglePause();
        GameManager.current.eventService.RequestUITogglePauseMenu(!GameManager.current.updateService.isGamePausedInspector);
    }

    public void ControllerInteract()
    {
        if (playerPawn == null) return;

        playerPawn.Interact();
    }

    public void ControllerInteractReset()
    {
        if (playerPawn == null) return;

        playerPawn.InteractReset();
    }

    #endregion
}
