using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action_Scripts;
using Items;
using UnityEngine;

namespace Power_Ups
{
    [CreateAssetMenu(menuName = "TileMatchingGame/PowerUps/ColourBomb")]
    public class ColourBomb : PowerUp
    {
        public override async Task Activate(Tile activationTile)
        {
            List<Tile> sameColours = new List<Tile>();

            foreach (var row in BoardController.Instance.rows)
            {
                sameColours.AddRange(row.tiles.Where(tile =>
                    !tile.itemIsUnderElimination &&
                    tile.Item is Jewel jewel &&
                    jewel.jewelType ==
                    ((Jewel)activationTile.Item).jewelType));
            }

            await EliminationAction.Instance.CommenceSpecialEliminationAction(sameColours);
        }
    }
}