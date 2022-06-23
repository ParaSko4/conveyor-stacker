using ConveyorStacker.GameplayCore;
using DG.Tweening;
using UnityEngine;

namespace ConveyorStacker.UI
{
    public class GuidHand : MonoBehaviour
    {
        [SerializeField]
        private float movementDuration;
        [SerializeField]
        private ItemsTaker itemsTaker;
        [SerializeField]
        private ParticleSystem scanPrefab;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                itemsTaker.TakeItemWithDelay(movementDuration);
                transform.DOMove(Input.mousePosition, movementDuration);

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Instantiate(scanPrefab, hit.point, Quaternion.identity);
                }
            }
        }
    }
}