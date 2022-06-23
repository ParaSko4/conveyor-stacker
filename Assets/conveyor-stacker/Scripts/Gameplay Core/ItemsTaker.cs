using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class ItemsTaker : MonoBehaviour
    {
        private const string ToyTag = "Toy";

        [SerializeField]
        private ItemsBoxManager itemsBoxManager;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        public async void TakeItemWithDelay(float delayTime)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(ToyTag))
                {
                    int itemsBoxIndex = 0;
                    var itemsBox = itemsBoxManager.NotPackedItemsBoxes.FirstOrDefault(itemsBox => hit.transform.name.Contains(itemsBox.ItemName));

                    while (itemsBox == null &&
                        itemsBoxIndex < itemsBoxManager.NotPackedItemsBoxes.Count)
                    {
                        itemsBox = itemsBoxManager.NotPackedItemsBoxes.ElementAt(itemsBoxIndex);

                        if (itemsBox != null && itemsBox.BoxFull)
                        {
                            itemsBox = null;
                        }

                        itemsBoxIndex++;
                    }

                    if (itemsBox == null)
                    {
                        return;
                    }

                    await UniTask.Delay((int)(delayTime * 1000));

                    itemsBox.PutInBoxItem(hit.transform);
                }
            }
        }
    }
}