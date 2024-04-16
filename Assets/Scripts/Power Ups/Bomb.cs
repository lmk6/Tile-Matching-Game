using System.Linq;
using System.Threading.Tasks;
using Action_Scripts;
using Items;
using UnityEngine;

namespace Power_Ups
{
    [CreateAssetMenu(menuName = "TileMatchingGame/PowerUps/Bomb")]
    public class Bomb : PowerUp
    {
        public override async Task Activate(Tile activationTile)
        {
            await EliminationAction.Instance.CommenceSpecialEliminationAction(
                activationTile.SurroundingTiles.ToList().Where(tile =>
                    tile is not null && tile.Item is Jewel && !tile.itemIsUnderElimination).ToList());
        }
    }
}