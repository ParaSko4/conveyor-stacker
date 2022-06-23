using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class Hammer : MonoBehaviour
    {
        [SerializeField]
        private float delay;
        [SerializeField]
        private float distance;
        [SerializeField]
        private float duration;

        private float startZ;

        private void Awake()
        {
            startZ = transform.position.z;
        }

        public async void Punch()
        {
            await UniTask.Delay((int)(delay * 1000f));

            transform.DOMoveZ(startZ - distance, duration).SetEase(Ease.InCubic)
                    .OnComplete(() =>
                    {
                        transform.DOMoveZ(startZ, duration).SetEase(Ease.OutCubic);
                    });
        }
    }
}