using DG.Tweening;
using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField]
        private float height;
        [SerializeField]
        private float showingDuration;
        [SerializeField]
        private float movementDuration;

        private Vector3 startScale;

        private void Awake()
        {
            startScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        private async void Start()
        {
            await transform.DOScale(startScale, showingDuration).AsyncWaitForCompletion();

            transform.DOShakeRotation(movementDuration, strength: 30f);

            await transform.DOLocalMoveZ(transform.position.z + height, movementDuration).AsyncWaitForCompletion();

            transform.DOScale(Vector3.zero, showingDuration);
        }
    }
}