using System.Threading.Tasks;
using UnityEngine;

namespace Power_Ups
{
    [CreateAssetMenu(menuName = "TileMatchingGame/PowerUps/Fragile")]
    public class Fragile : PowerUp
    {
        public override Task Activate(Tile tile)
        {
            return Task.CompletedTask;
        }
    }
}
