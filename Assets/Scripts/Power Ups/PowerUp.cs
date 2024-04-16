using System.Threading.Tasks;
using UnityEngine;

namespace Power_Ups
{
    public abstract class PowerUp : ScriptableObject
    {
        public Sprite sprite;
        public virtual Task Activate(Tile tile)
        {
            return Task.CompletedTask;
        }
    }
}
