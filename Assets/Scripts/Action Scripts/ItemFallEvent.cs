using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Action_Scripts
{
    /*
     * Item fall handler
     */
    public sealed class ItemFallEvent : MonoBehaviour
    {
        public static ItemFallEvent Instance { get; private set; }


        // public float animationSpeed = 1f;

        private void Awake()
        {
            Instance = this;
        }

        public async Task CommenceFall(List<Tile> pathToDestination)
        {
            await BoardController.Instance.DisableInteraction();
            Fall(pathToDestination);
            await BoardController.Instance.EnableInteraction();
        }

        private void Fall(List<Tile> pathToDestination)
        {
            var source = pathToDestination.First();
            var destination = pathToDestination.Last();
        
            var srcIcon = source.tileIcon;
            var destIcon = destination.tileIcon;

            var srcIconTransform = srcIcon.transform;
            var destIconTransform = destIcon.transform;

            var srcPos = srcIconTransform.position;
            var destPos = destIconTransform.position;

            srcIconTransform.position = destPos;
            destIconTransform.position = srcPos;

            srcIconTransform.SetParent(destination.transform);
            destIconTransform.SetParent(source.transform);

            source.tileIcon = destIcon;
            destination.tileIcon = srcIcon;
        
            destination.Item = source.Item;
            source.Item = null;
        }
    }
}