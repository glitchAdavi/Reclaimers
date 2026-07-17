using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] Pawn player;

    [SerializeField] bool controlEnabledAll = true;
    [SerializeField] bool controlEnabledPlayer = false;
    [SerializeField] bool isPaused = false;

    private void Awake()
    {
        this.Register();

        GameManager.Instance.eventSystem.onEnableAllControls += EnableAllControls;
        GameManager.Instance.eventSystem.onEnablePlayerControls += EnablePlayerControls;

    }

    public void ManagedUpdate()
    {

        if (!controlEnabledAll) return;


        if (Input.GetKeyDown(KeyCode.Escape)) PlayerEscape();


        if (!controlEnabledPlayer || isPaused) return;

        PlayerMove();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            PlayerFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerReload();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerAbility();
        }

        if (Input.GetKey(KeyCode.F))
        {
            PlayerInteract();
        }
        else PlayerInteractReset();

    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;
    }

    public void AssignPlayerPawn(Pawn pawn)
    {
        player = pawn;
    }

    public void EnablePlayerControls(bool active)
    {
        controlEnabledPlayer = active;
    }

    public void EnableAllControls(bool active)
    {
        controlEnabledAll = active;
    }

    #region Controls
    public void PlayerMove()
    {
        if (player == null) return;

        player.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public void PlayerFire()
    {
        if (player == null) return;

        player.WeaponFire();
    }

    public void PlayerReload()
    {
        if (player == null) return;

        player.WeaponReload();
    }

    public void PlayerAbility()
    {
        if (player == null) return;

        player.UseAbility();
    }

    public void PlayerInteract()
    {
        if (player == null) return;

        player.Interact();
    }

    public void PlayerInteractReset()
    {
        if (player == null) return;

        player.InteractReset();
    }

    public void PlayerEscape()
    {
        GameManager.Instance.eventSystem.OpenCloseMenues();
    }

    #endregion
}
