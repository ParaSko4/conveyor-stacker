using UnityEngine;

namespace ConveyorStacker.GameplayCore
{
    public class SceneBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ItemsBoxManager itemsBoxManager;
        [SerializeField]
        private Hammer hammer;
        [SerializeField]
        private ParticleSystem sadSmail;
        [SerializeField]
        private Bonus bonusPrefab;

        private void Awake()
        {
            itemsBoxManager.BoxInConveyorEnd += OnBoxInConveyorEnd;
        }

        private void OnDestroy()
        {
            itemsBoxManager.BoxInConveyorEnd -= OnBoxInConveyorEnd;
        }

        private void OnBoxInConveyorEnd(ItemsBox itemsBox)
        {
            if (itemsBox.BoxCorrect == false)
            {
                hammer.Punch();
                sadSmail.Play();
            }
            else
            {
                Instantiate(bonusPrefab);
            }
        }
    }
}