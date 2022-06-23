using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class Conveyour : MonoBehaviour
    {
        private const float TextureSpeedMultiplier = 10f;

        [SerializeField]
        private float speed;

        private Rigidbody rb;
        private Material material;
        private float conveyorWorkTimer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            material = GetComponent<MeshRenderer>().material;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            conveyorWorkTimer += Time.deltaTime;

            material.mainTextureOffset = new Vector2(conveyorWorkTimer * Time.deltaTime * speed * TextureSpeedMultiplier, 0f);
            var startRigidbodyPosition = rb.position;
            rb.position -= speed * Time.deltaTime * Vector3.right;
            rb.MovePosition(startRigidbodyPosition);
        }
    }
}