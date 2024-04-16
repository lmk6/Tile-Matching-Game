using System.Collections.Generic;
using System.Threading.Tasks;
using Power_Ups;
using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        public bool canFall;
        public Sprite sprite;
        public bool canBeSwapped;
        public PowerUp powerUp;
        // public bool isTerminable;

        public virtual List<Tile> GetFallingPath(Tile parentTile)
        {
            return new List<Tile>();
        }

        public virtual Task ActivatePowerUp(Tile tile)
        {
            return Task.CompletedTask;
        }
    }
}