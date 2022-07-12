using FishNet.Object;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private float weaponRange;
    [SerializeField] private Animator muzzleFlashAnimator;
    [SerializeField] private LayerMask layerMask;
    [SerializeField]
    private bool _clientAuth = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_clientAuth)
                Shot();
            else
                RpcShot();
        }
    }

    [ServerRpc]
    private void RpcShot()
    {
        Shot();
    }

    private void Shot()
    {
        //muzzleFlashAnimator.SetTrigger("Shoot");
        bool isHit = Physics.Raycast(gunPoint.position, transform.forward, out var hit, weaponRange, layerMask);

        var trail = Instantiate(
            bulletTrail,
            gunPoint.position,
            transform.rotation
            );

        var trailScript = trail.GetComponent<BulletTrail>();

        if (isHit)
        {
            trailScript.SetTargetPosition(hit.point);
            //var hittable = hit.collider.GetComponent<IHittable>();
            //hittable?.Hit();
        }
        else
        {
            var endPosition = gunPoint.position + transform.forward * weaponRange;
            trailScript.SetTargetPosition(endPosition);
        }
    }
}
