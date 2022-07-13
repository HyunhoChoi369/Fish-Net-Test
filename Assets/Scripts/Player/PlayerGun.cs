using FishNet.Object;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private float weaponRange;
    [SerializeField] private Animator muzzleFlashAnimator;
    [SerializeField] private bool _clientAuth = true;

    private LayerMask EnemyLayer;
    private LayerMask OwnerLayer;
    private LayerMask PlayerLayer;
    private PlayerController playerController;
    private bool IsDead => playerController.IsDead;

    private void Awake()
    {
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        OwnerLayer = 1 << LayerMask.NameToLayer("Owner");
        PlayerLayer = 1 << LayerMask.NameToLayer("Player");

        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (IsDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            RpcShot();
        }
    }

    [ServerRpc]
    private void RpcShot()
    {
        Shot();
    }

    [Server]
    private void Shot()
    {
        bool isHit = Physics.Raycast(gunPoint.position, transform.forward, out var hit, weaponRange, PlayerLayer);

        DrawShot();

        if (isHit)
        {
            hit.rigidbody.GetComponent<Hittable>().Hit(10);
        }
    }

    [ObserversRpc]
    private void DrawShot()
    {
        //muzzleFlashAnimator.SetTrigger("Shoot");
        bool isHit = Physics.Raycast(gunPoint.position, transform.forward, out var hit, weaponRange, EnemyLayer);

        var trail = Instantiate(
            bulletTrail,
            gunPoint.position,
            transform.rotation
            );

        var trailScript = trail.GetComponent<BulletTrail>();

        if (isHit)
        {
            trailScript.SetTargetPosition(hit.point);
        }
        else
        {
            var endPosition = gunPoint.position + transform.forward * weaponRange;
            trailScript.SetTargetPosition(endPosition);
        }
    }
}
