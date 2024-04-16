using System.Collections.Generic;
using System.Threading.Tasks;
using Power_Ups;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "TileMatchingGame/Items/Jewel")]
    public class Jewel : Item
    {
        public JewelType jewelType;

        public override List<Tile> GetFallingPath(Tile parentTile)
        {
            List<Tile> path = new List<Tile>();
            var bottomNeighbour = parentTile.BottomNeighbourTile;
            path.Add(parentTile);

            while (bottomNeighbour is not null)
            {
                if (bottomNeighbour.Item is not null) break;
                path.Add(bottomNeighbour);
                bottomNeighbour = bottomNeighbour.BottomNeighbourTile;
            }

            return path.Count == 1 ? new List<Tile>() : path;
        }

        public override async Task ActivatePowerUp(Tile tile)
        {
            if (powerUp is not null)
                await powerUp.Activate(tile);
        }
    }
}