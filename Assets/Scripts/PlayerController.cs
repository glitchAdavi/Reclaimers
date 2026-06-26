using Reclaimers.UpdateSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] Pawn player;

    [SerializeField] bool controlEnabledAll = true;
    [SerializeField] bool controlEnabledPlayer = true;
    [SerializeField] bool isPaused = false;

    private void Awake()
    {
        this.Register();
    }

    public void ManagedUpdate()
    {





        if (!controlEnabledPlayer || isPaused) return;


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

    #endregion
}
