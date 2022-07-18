using FishNet.Object;
using FishNet.Object.Synchronizing;
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
    private float shotDelay = 0;
    private PlayerBase playerBase;

    [SyncVar(OnChange = nameof(OnCurGunDataChanged))] public GunData curGunData = null;

    private void OnCurGunDataChanged(GunData oldValue, GunData newValue, bool asServer)
    {
        if (IsClient)
        {
            Debug.Log($"Change Gun : {newValue.name}");
            // 그림 바꾸기
        }
    }

    private bool IsDead => playerController.IsDead;

    private void Awake()
    {
        EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        OwnerLayer = 1 << LayerMask.NameToLayer("Owner");
        PlayerLayer = 1 << LayerMask.NameToLayer("Player");

        playerController = GetComponent<PlayerController>();
        playerBase = GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (IsDead) return;

        shotDelay += Time.deltaTime;

        if (shotDelay > curGunData.ShotDelay && Input.GetMouseButton(0))
        {
            RpcShot();
            DrawShot();
            shotDelay = 0f;
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
        var dir = transform.forward;
        var spreadX = Random.Range(-curGunData.Spread, curGunData.Spread);
        //var spreadY = Random.Range(-curGunData.Spread, curGunData.Spread);
        dir = Quaternion.AngleAxis(spreadX, transform.up) * dir;
        //dir = Quaternion.AngleAxis(spreadY, transform.right) * dir;

        bool isHit = Physics.Raycast(gunPoint.position, dir, out var hit, weaponRange, PlayerLayer);

        if (isHit)
        {
            hit.rigidbody.GetComponent<Hittable>().Hit(10);
        }
    }

    private void DrawShot()
    {
        //muzzleFlashAnimator.SetTrigger("Shoot");
        //playerBase.PlayShot();

        var dir = transform.forward;
        var spreadX = Random.Range(-curGunData.Spread, curGunData.Spread);
        dir = Quaternion.AngleAxis(spreadX, transform.up) * dir;

        bool isHit = Physics.Raycast(gunPoint.position, dir, out var hit, weaponRange, EnemyLayer);

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
            var endPosition = gunPoint.position + dir * weaponRange;
            trailScript.SetTargetPosition(endPosition);
        }
    }
}
