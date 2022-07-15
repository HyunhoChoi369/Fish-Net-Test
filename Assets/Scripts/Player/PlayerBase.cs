using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Hittable
{
    public int Life = 100;
    public Animator animator;

    private int enemyLayer;
    private int ownerLayer;
    private int playerLayer;
    private PlayerController playerController;
    private int deadAnim = Animator.StringToHash("Dead");
    private int shotAnim = Animator.StringToHash("Shot_Bust");
    private int hitAnim = Animator.StringToHash("Hit");
    private int runBlend = Animator.StringToHash("RunBlend");

    private Collider capsule;



    private void Awake()
    {
        enemyLayer = LayerMask.NameToLayer("Enemy");
        ownerLayer = LayerMask.NameToLayer("Owner");
        playerLayer = LayerMask.NameToLayer("Player");
        playerController = GetComponent<PlayerController>();
        capsule = GetComponent<Collider>();

        PlayerManager.Instance.AddPlayer(this);
    }

    public override void Hit(int damage)
    {
        RpcHit();
        Life -= damage;

        if (Life <= 0)
        {
            Dead();
            RpcDead();
        }
    }

    [ObserversRpc] private void RpcDead() => animator.Play(deadAnim);
    [ObserversRpc] private void RpcHit() => animator.Play(hitAnim);

    private void Dead()
    {
        playerController.IsDead = true;
        capsule.enabled = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsClient)
        {
            if (base.IsOwner)
            {
                gameObject.layer = ownerLayer;
                GetComponent<Invisible>().AlwaysVisible = true;
            }
            else
            {
                gameObject.layer = enemyLayer;
            }
        }
    }



    [ServerRpc]
    public void GetItem(byte playerIndex, ItemType itemType)
    {
        Debug.Log($"player{ClientManager.Connection} is got {itemType}");
    }

    [Client]
    public void PlayShot()
    {
        animator.Play(shotAnim);
    }

    [Client]
    public void PlayRun(float blend)
    {
        animator.SetFloat(runBlend, blend);
    }
}


