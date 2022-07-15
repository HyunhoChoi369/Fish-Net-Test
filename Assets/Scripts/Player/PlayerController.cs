using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float _moveRate = 4f;
    [SerializeField]
    private bool _clientAuth = true;

    private Camera camera;
    private PlayerBase playerBase;
    private Rigidbody rb;

    [SyncVar] public bool IsDead = false;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            camera = GameObject.Find("Camera").GetComponent<Camera>();
            PlayerManager.Instance.PlayerCamera.Follow = transform;
            PlayerManager.Instance.PlayerCamera.LookAt = transform;
        }
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        if (IsDead) return;

        var dir = transform.forward;

        if (IsClient)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, float.MaxValue, layerMask);
            dir = hit.point;
            dir.y = transform.position.y;
        }

        //Vector3 mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.y));


        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        /* If ground cannot be found for 20 units then bump up 3 units. 
         * This is just to keep player on ground if they fall through
         * when changing scenes.             */
        if (_clientAuth || (!_clientAuth && base.IsServer))
        {
            if (!Physics.Linecast(transform.position + new Vector3(0f, 0.3f, 0f), transform.position - (Vector3.one * 20f)))
                transform.position += new Vector3(0f, 3f, 0f);
        }

        if (_clientAuth)
            Move(hor, ver, dir);
        else
            RpcMove(hor, ver, dir);
    }

    [ServerRpc]
    private void RpcMove(float hor, float ver, Vector3 dir)
    {
        Move(hor, ver, dir);
    }

    private void Move(float hor, float ver, Vector3 dir)
    {
        if (IsDead) return;

        transform.LookAt(dir);

        Vector3 direction = new Vector3(hor, 0f, ver) * _moveRate;
        rb.velocity = direction;

        if (hor == 0f && ver == 0f)
        {
            playerBase.PlayRun(0);
            return;
        }

        var forward = Vector3.Dot(transform.forward, direction);
        var right = Vector3.Dot(transform.right, direction);

        if (Mathf.Abs(forward) >= Mathf.Abs(right))
        {
            if (forward > 0)
            {
                playerBase.PlayRun(1);
            }

            if (forward < 0)
            {
                playerBase.PlayRun(3);
            }
        }
        else
        {
            if (right > 0)
            {
                playerBase.PlayRun(2);
            }

            if (right < 0)
            {
                playerBase.PlayRun(4);
            }
        }
    }
}
