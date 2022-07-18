using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Survival.Ingame.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private float _moveRate = 4f;
        [SerializeField]
        private float _rotateRate = 1f;

        private Camera camera;
        private Rigidbody rb;

        [SyncVar, HideInInspector] public bool IsDead = false;

        private void Awake()
        {
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
            if (IsServerOnly) return;
            if (!base.IsOwner) return;
            if (IsDead) return;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, float.MaxValue, layerMask);
            var point = hit.point;
            point.y = transform.position.y;

            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");


            if (!Physics.Linecast(transform.position + new Vector3(0f, 0.3f, 0f), transform.position - (Vector3.one * 20f)))
                transform.position += new Vector3(0f, 3f, 0f);

            Move(hor, ver, point);
        }

        private void Move(float hor, float ver, Vector3 point)
        {
            if (IsDead) return;

            //transform.LookAt(point);
            var targetRot = Quaternion.LookRotation(point - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, _rotateRate * Time.deltaTime);

            Vector3 direction = new Vector3(hor, 0f, ver) * _moveRate;
            rb.velocity = direction;
        }
    }
}