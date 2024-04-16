using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Action_Scripts
{
    /**
     * Handles the Swap action
     */
    public sealed class SwapJewels : MonoBehaviour
    {
        public static SwapJewels Instance;
        public float animationMoveSpeed = 0.3f;
        private TaskCompletionSource<bool> _completionStatus;

        private Tile _swipedTile;
        private Tile _tileToSwipeWith;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Vector2 _dragVector;

        private GraphicRaycaster _raycaster;
        private EventSystem _eventSystem;
        private PointerEventData _pointerEventData;

        private void Awake()
        {
            Instance = this;
            _raycaster = GetComponent<GraphicRaycaster>();
            _eventSystem = GetComponent<EventSystem>();
        }

        /**
         * Checks which tile the drag started on
         */
        public void OnPressed()
        {
            if (_completionStatus is not null) return;
            _swipedTile = null;
            _tileToSwipeWith = null;
            GetTilePressedOn();
        }

        /**
         * Checks which tile the drag ended on
         */
        public void OnReleased()
        {
            if (_swipedTile is null || _completionStatus is not null) return;
            _endPosition = Input.mousePosition;
            _dragVector = _endPosition - _startPosition;
            CommenceSwipe();
        }

        private void GetTilePressedOn()
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _startPosition = Input.mousePosition;
            _pointerEventData.position = _startPosition;

            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);

            foreach (var result in results)
            {
                if (!result.gameObject.CompareTag("Tile")) continue;
                var tempTileHolder = result.gameObject.GetComponent<Tile>();
                if (!tempTileHolder.IsTileSwappable()) continue;
                _swipedTile = tempTileHolder;
            }
        }

        /**
         * Final checks before and swipe action start
         */
        private async void CommenceSwipe()
        {
            DetermineTileToSwipeWith();
            if (_tileToSwipeWith is null 
                || _swipedTile is null
                || _tileToSwipeWith.HasMatchingSet()) return;


            await Swap();
            _completionStatus = null;
        }

        /**
         * Simple estimation of the direction in which the player swiped 
         */
        private void DetermineTileToSwipeWith()
        {
            var absX = Mathf.Abs(_dragVector.x);
            var absY = Mathf.Abs(_dragVector.y);

            if (absX > absY)
            {
                _tileToSwipeWith = _dragVector.x < 0 ? _swipedTile.LeftNeighbourTile : _swipedTile.RightNeighbourTile;
            }
            else
            {
                _tileToSwipeWith = _dragVector.y > 0 ? _swipedTile.TopNeighbourTile : _swipedTile.BottomNeighbourTile;
            }

            _tileToSwipeWith = _tileToSwipeWith is { Item: Jewel } ? _tileToSwipeWith : null;
        }

        /**
         * Logic calculated / modified first, animation run as a coroutine
         */
        private async Task Swap()
        {
            _completionStatus = new TaskCompletionSource<bool>();
        
            var icon1 = _swipedTile.tileIcon;
            var icon2 = _tileToSwipeWith.tileIcon;

            var icon1Transform = icon1.transform;
            var icon2Transform = icon2.transform;

            await BoardController.Instance.DisableInteraction();

            StartCoroutine(SwapTwoAnimation(icon1Transform, icon2Transform, _completionStatus));

            await _completionStatus.Task;
        
            icon1Transform.SetParent(_tileToSwipeWith.transform);
            icon2Transform.SetParent(_swipedTile.transform);

            _swipedTile.tileIcon = icon2;
            _tileToSwipeWith.tileIcon = icon1;

            (_swipedTile.Item, _tileToSwipeWith.Item) = (_tileToSwipeWith.Item, _swipedTile.Item);
            (_tileToSwipeWith, _swipedTile) = (null, null);
            TileMatchingGame.Instance.UseSwipe();
            await BoardController.Instance.EnableInteraction();
        }

        private IEnumerator SwapTwoAnimation(Transform transform1, Transform transform2,
            TaskCompletionSource<bool> completionSource)
        {
            var pos1 = transform1.position;
            var pos2 = transform2.position;
            float elapsedTime = 0;

            while (elapsedTime < animationMoveSpeed)
            {
                elapsedTime += Time.deltaTime;
                var percentageComplete = elapsedTime / animationMoveSpeed;
                transform1.position = Vector3.Lerp(pos1, pos2, Mathf.SmoothStep(0, 1, percentageComplete));
                transform2.position = Vector3.Lerp(pos2, pos1, Mathf.SmoothStep(0, 1, percentageComplete));
                yield return null;
            }

            completionSource.SetResult(true);
        }
    }
}