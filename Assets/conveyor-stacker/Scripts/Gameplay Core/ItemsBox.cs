using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class ItemsBox : MonoBehaviour
    {
        private class BoxItem
        {
            public bool correct;
            public Transform transform;
            public Vector3 startPosition;
        }

        [SerializeField]
        private float boxSize;
        [SerializeField]
        private string itemName;
        [Space(10)]
        [SerializeField]
        private float puttingHeight;
        [SerializeField]
        private float itemMovementDuration;
        [Space(10)]
        [SerializeField]
        private Transform boxCover;
        [SerializeField]
        private Transform itemsDropPoint;
        [SerializeField]
        private SpriteRenderer itemSpriteRenderer;
        [SerializeField]
        private SpriteRenderer boxCoverSpriteRenderer;
        [SerializeField]
        private TextMeshPro boxSizeLabel;
        [Space(10)]
        [SerializeField]
        private Sprite itemSprite;
        [SerializeField]
        private Sprite wrongSticker;
        [SerializeField]
        private Sprite correctSticker;
        [Space(10)]
        [SerializeField]
        private float coverShowingDuration;
        [SerializeField]
        private float coverDescentDuration;
        [SerializeField]
        private float coverShowingHeight;

        private Stack<BoxItem> itemsInBox = new Stack<BoxItem>();
        private float startCoverHeight;

        public string ItemName => itemName;
        public bool BoxCorrect => itemsInBox.All(iib => iib.correct) && BoxFull;
        public bool BoxFull => itemsInBox.Count >= boxSize;

        private void Awake()
        {
            itemSpriteRenderer.sprite = itemSprite;
            startCoverHeight = boxCover.localPosition.y;
            boxCover.localScale = Vector3.zero;
            boxCover.localPosition = boxCover.localPosition.ChangeY(startCoverHeight + coverShowingHeight);
        }

        private void Start()
        {
            boxSizeLabel.text = $"{itemsInBox.Count}/{boxSize}";
        }

        public void PutInBoxItem(Transform itemTransform)
        {
            var boxItem = new BoxItem();
            boxItem.correct = itemTransform.name.Contains(itemName);
            boxItem.transform = itemTransform;
            boxItem.startPosition = itemTransform.position;

            itemsInBox.Push(boxItem);

            boxSizeLabel.text = $"{itemsInBox.Count}/{boxSize}";

            float distance = Vector3.Distance(itemsDropPoint.position, itemTransform.position);
            float twentyPercent = distance * 0.2f;
            var startItemPosition = itemTransform.position;

            DOVirtual.Float(0f, 1f, itemMovementDuration, (t) =>
            {
                var direction = (itemsDropPoint.position - startItemPosition).normalized;

                var newPoint = Bezier.GetPoint(itemTransform.position,
                    (startItemPosition + direction * twentyPercent).ChangeY(puttingHeight),
                    (startItemPosition + direction * (distance - twentyPercent)).ChangeY(puttingHeight),
                    itemsDropPoint.position,
                    t);
                itemTransform.position = newPoint;
            }).SetEase(Ease.Linear).OnComplete(() =>
            {
                itemTransform.position = itemsDropPoint.position;
            });
        }

        public void TakeLastItemBack()
        {
            //itemsInBox
        }

        public async void CloseBox()
        {
            boxCoverSpriteRenderer.sprite = wrongSticker;

            if (BoxCorrect)
            {
                boxCoverSpriteRenderer.sprite = correctSticker;
            }

            boxSizeLabel.DOFade(0f, 1f);

            await boxCover.DOScale(Vector3.one, coverShowingDuration).AsyncWaitForCompletion();

            boxCover.DOLocalMoveY(startCoverHeight, coverDescentDuration);
        }
    }
}
