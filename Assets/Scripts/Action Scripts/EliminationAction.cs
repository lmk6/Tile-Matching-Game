using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Action_Scripts
{
    /**
     * Elimination action handler
     */
    public sealed class EliminationAction : MonoBehaviour
    {
        public static EliminationAction Instance { get; private set; }
        private List<Task> _animationRunTasks;
        public float animationSpeed = 0.3f;


        private void Awake()
        {
            Instance = this;
        }

        /*
         * Standard Elimination of a matched set of Jewels
         */
        public async Task CommenceEliminationAction(List<Tile> matchedTiles)
        {
            await BoardController.Instance.DisableInteraction();
            await Eliminate(matchedTiles);
            await BoardController.Instance.EnableInteraction();
        }

        /*
         * Special elimination - don't wait until the animation is done
         */
        public async Task CommenceSpecialEliminationAction(List<Tile> tilesToEliminate)
        {
            await Eliminate(tilesToEliminate, false);
        }

        private async Task Eliminate(List<Tile> tilesWithJewelsToEliminate, bool isMain = true)
        {
            _animationRunTasks = new List<Task>();

            foreach (var tile in tilesWithJewelsToEliminate)
            {
                tile.itemIsUnderElimination = true;
            }
            
            foreach (var tile in tilesWithJewelsToEliminate)
            {
                _animationRunTasks.Add(RunAnimation(tile));
                await tile.EliminateItem();
            }

            await Task.WhenAll(_animationRunTasks);

            if (isMain)
                TriggerFall();
        }

        private void TriggerFall()
        {
            var lowestInColumn = BoardController.Instance.rows.Last().tiles;

            foreach (var tile in lowestInColumn)
            {
                tile.TriggerNeighbourItemFall();
            }
        }

        private async Task RunAnimation(Tile tile)
        {
            var singleTaskCompletion = new TaskCompletionSource<bool>();

            StartCoroutine(EliminationAnimation(tile, singleTaskCompletion));

            await singleTaskCompletion.Task;
        }

        private IEnumerator EliminationAnimation(Tile tile, TaskCompletionSource<bool> tcs)
        {
            var tileIcon = tile.tileIcon;
            var initScale = tileIcon.transform.localScale;
            var desiredScale = new Vector2(0, 0);
            float elapsedTime = 0;

            while (elapsedTime < animationSpeed)
            {
                elapsedTime += Time.deltaTime;
                var percentageComplete = elapsedTime / animationSpeed;
                tileIcon.transform.localScale =
                    Vector2.Lerp(initScale, desiredScale, Mathf.SmoothStep(0, 1, percentageComplete));
                yield return null;
            }

            tcs.SetResult(true);
        }
    }
}