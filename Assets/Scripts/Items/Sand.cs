using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "TileMatchingGame/Items/Sand")]
    public class Sand : Blocker
    {

        public override List<Tile> GetFallingPath(Tile parentTile)
        {
            var fullPath = new List<Tile>();
            var newPathPart = new List<Tile>() { parentTile };
            var nextStartingTile = parentTile;

            do
            {
                fullPath.AddRange(newPathPart);

                newPathPart = GetComputedPath(nextStartingTile);
                if (newPathPart is not null)
                    nextStartingTile = newPathPart.Last();
            } while (newPathPart is not null);

            return fullPath.Count > 1 ? fullPath : new List<Tile>();
        }

        private List<Tile> GetComputedPath(Tile startingTile)
        {
            var pathDown = new List<Tile> { startingTile.BottomNeighbourTile };
            var pathLeft = startingTile.LeftNeighbourTile is { BottomNeighbourTile: not null }
                ? new List<Tile> { startingTile.LeftNeighbourTile, startingTile.LeftNeighbourTile.BottomNeighbourTile }
                : null;
            var pathRight = startingTile.RightNeighbourTile is { BottomNeighbourTile: not null }
                ? new List<Tile> { startingTile.RightNeighbourTile, startingTile.RightNeighbourTile.BottomNeighbourTile }
                : null;

            var paths = new List<List<Tile>>
            {
                pathDown, pathLeft, pathRight
            }.Where(path => path is not null).ToList();

            foreach (var path in paths)
            {
                if (path.Any(item => item is null)) continue;
                if (path.All(item => item.Item is null)) return path;
            }

            return null;
        }
    }
}