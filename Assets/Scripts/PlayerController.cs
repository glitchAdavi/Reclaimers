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

        EventSystem.Instance.onEnablePlayerControls += EnablePlayerControls;
    }

    public void ManagedUpdate()
    {

        if (!controlEnabledAll) return;





        if (!controlEnabledPlayer || isPaused) return;




        if (Input.GetKey(KeyCode.Mouse0))
        {
            PlayerFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerReload();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerInteract();
        }

        PlayerMove();

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

    public void PlayerInteract()
    {
        if (player == null) return;


    }

    #endregion
}
