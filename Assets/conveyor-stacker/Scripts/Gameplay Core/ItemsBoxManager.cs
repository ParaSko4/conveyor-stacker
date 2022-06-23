using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class ItemsBoxManager : MonoBehaviour
    {
        public event Action<ItemsBox> BoxInConveyorEnd;

        [SerializeField]
        private ItemsBox[] itemsBoxes;
        [SerializeField]
        private Transform conveyorStart;
        [SerializeField]
        private Transform conveyorEnd;

        private Queue<ItemsBox> itemsBoxesQueue;

        public LinkedList<ItemsBox> NotPackedItemsBoxes { get; private set; } = new LinkedList<ItemsBox>();

        private void Awake()
        {
            itemsBoxesQueue = new Queue<ItemsBox>(itemsBoxes
                .OrderBy(itemsBox => Vector3.Distance(conveyorEnd.transform.position, itemsBox.transform.position)));
        }

        private void Start()
        {
            WaitStartAsync();
            WaitEndAsync();
        }

        private async void WaitStartAsync()
        {
            while (itemsBoxesQueue.Count != 0)
            {
                var itemsBox = itemsBoxesQueue.Dequeue();

                await UniTask.WaitUntil(() => Vector3.Distance(conveyorStart.position, itemsBox.transform.position) < 0.05f);

                NotPackedItemsBoxes.AddLast(itemsBox);
            }
        }

        private async void WaitEndAsync()
        {
            await UniTask.WaitUntil(() => NotPackedItemsBoxes.Count != 0);

            while (itemsBoxesQueue.Count != 0 || NotPackedItemsBoxes.Count != 0)
            {
                var itemsBox = NotPackedItemsBoxes.First();

                await UniTask.WaitUntil(() => Vector3.Distance(conveyorEnd.position, itemsBox.transform.position) < 0.1f);

                NotPackedItemsBoxes.RemoveFirst();
                itemsBox.CloseBox();
                BoxInConveyorEnd?.Invoke(itemsBox);

            }
        }
    }
}