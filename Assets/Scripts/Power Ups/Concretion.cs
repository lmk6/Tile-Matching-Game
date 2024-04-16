using System.Threading.Tasks;
using Items;
using UnityEngine;

namespace Power_Ups
{
    [CreateAssetMenu(menuName = "TileMatchingGame/PowerUps/Concretion")]
    public class Concretion : PowerUp
    {
        public override Task Activate(Tile tile)
        {
            tile.Item = CreateInstance<Blocker>();
            return Task.CompletedTask;
        }
    }
}
