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

    private int EnemyLayer;
    private int OwnerLayer;
    private int PlayerLayer;
    private PlayerController playerController;
    private int deadHash = Animator.StringToHash("Dead");


    private void Awake()
    {
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        OwnerLayer = LayerMask.NameToLayer("Owner");
        PlayerLayer = LayerMask.NameToLayer("Player");
        playerController = GetComponent<PlayerController>();
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

    [ObserversRpc] private void RpcDead() => animator.Play(deadHash);
    [ObserversRpc] private void RpcHit() => animator.SetTrigger("Hit");

    private void Dead()
    {
        playerController.IsDead = true;
        //animator.SetBool("Dead", true);
        //animator.Play(deadHash);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsClient)
        {
            if (base.IsOwner)
            {
                gameObject.layer = OwnerLayer;
                GetComponent<Invisible>().AlwaysVisible = true;
            }
            else
            {
                gameObject.layer = EnemyLayer;
            }
        }
    }
}


