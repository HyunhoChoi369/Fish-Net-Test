using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survival.Ingame.Bullet
{
    public class BulletTrail : MonoBehaviour
    {
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float progress;

        [SerializeField] private float speed = 40f;

        // Start is called before the first frame update
        private void Start()
        {
            startPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            progress += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }
    }
}