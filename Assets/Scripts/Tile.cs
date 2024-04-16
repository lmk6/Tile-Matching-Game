using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action_Scripts;
using Items;
using Power_Ups;
using UnityEngine;
using UnityEngine.UI;

/*
 * Single Grid Tile
 */
public sealed class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;

    public Image tileIcon;

    public Button button;

    public Color idleColour;

    public Color matchingTileHighlightColour;

    public bool itemIsUnderElimination;

    private bool _swapEventEnabled = true;

    private List<Tile> _terminableTiles;

    private Image _tileBackground;


    public bool SwapEventEnabled
    {
        get => _swapEventEnabled;

        set
        {
            if (IsTileSwappable() && TileMatchingGame.Instance.SwipeAllowed())
                _swapEventEnabled = value;
            else _swapEventEnabled = false;
        }
    }

    private void Awake()
    {
        _tileBackground = transform.Find("Background").GetComponent<Image>();
    }

    private void Start()
    {
        AddBtnFunctionality();
        _terminableTiles = new List<Tile>();
    }

    private void Update()
    {
        _swapEventEnabled = TileMatchingGame.Instance.SwipeAllowed();
    }

    private void AddBtnFunctionality()
    {
        button.onClick.AddListener(EliminateMatched);
    }

    private async void EliminateMatched()
    {
        SetMatchedHighlight(false);
        await EliminationAction.Instance.CommenceEliminationAction(_terminableTiles);
    }

    public Item Item
    {
        get => _item;

        set
        {
            if (_item == value) return;
            _item = value;

            SetupSprites(_item);
            SetHighlight(false);

            switch (_item)
            {
                case Jewel:
                    FindAllMatchingTiles();
                    button.enabled = true;
                    break;
                case null:
                    TriggerNeighbourItemFall();
                    goto default;
                default:
                    button.enabled = false;
                    _terminableTiles = new List<Tile>();
                    break;
            }
        }
    }

    /**
     * Triggers Falling action after Elimination
     * Prioritises Sand's fall over jewel's
     *
     * Function is recursive and 'climbs' up the column
     */
    public void TriggerNeighbourItemFall()
    {
        var neighboursAbleToTrigger = new List<Tile>() { LeftNeighbourTile, RightNeighbourTile, TopNeighbourTile };

        foreach (var neighbour in neighboursAbleToTrigger)
        {
            if (neighbour is null || neighbour.Item is null) continue;
            neighbour.ItemFallTry();
        }

        if (TopNeighbourTile is not null) TopNeighbourTile.TriggerNeighbourItemFall();
    }

    public async Task EliminateItem()
    {
        _terminableTiles = new List<Tile>();
        if (_item is null) return;
        await _item.ActivatePowerUp(this);
        if (_item is Jewel)
            _item = null;
        SetupSprites(_item);
        itemIsUnderElimination = false;
        SetMatchedHighlight(false);
    }

    public bool HasMatchingSet()
    {
        FindAllMatchingTiles();
        return _terminableTiles.Count > 0;
    }
    
    /**
     * Neighbour Logic Section
     *
     * Idea is that instead of using x and y, all checks are done using tile's neighbours
     */

    public Tile TopNeighbourTile => y > 0 ? BoardController.Instance.GridOfTiles[x, y - 1] : null;

    public Tile BottomNeighbourTile =>
        y < BoardController.Instance.BoardHeight() - 1 ? BoardController.Instance.GridOfTiles[x, y + 1] : null;

    public Tile LeftNeighbourTile => x > 0 ? BoardController.Instance.GridOfTiles[x - 1, y] : null;

    public Tile RightNeighbourTile =>
        x < BoardController.Instance.BoardWidth() - 1 ? BoardController.Instance.GridOfTiles[x + 1, y] : null;

    public Tile TopLeftNeighbourTile =>
        x > 0 && y > 0 ? BoardController.Instance.GridOfTiles[x - 1, y - 1] : null;

    public Tile BottomLeftNeighbourTile =>
        x > 0 && y < BoardController.Instance.BoardHeight() - 1
            ? BoardController.Instance.GridOfTiles[x - 1, y + 1]
            : null;

    public Tile TopRightNeighbourTile =>
        x < BoardController.Instance.BoardWidth() - 1 &&
        y > 0
            ? BoardController.Instance.GridOfTiles[x + 1, y - 1]
            : null;

    public Tile BottomRightNeighbourTile =>
        x < BoardController.Instance.BoardWidth() - 1 && y < BoardController.Instance.BoardHeight() - 1
            ? BoardController.Instance.GridOfTiles[x + 1, y + 1]
            : null;

    public Tile[] SurroundingTiles => new[]
    {
        TopNeighbourTile,
        RightNeighbourTile,
        BottomNeighbourTile,
        LeftNeighbourTile,
        TopLeftNeighbourTile,
        BottomLeftNeighbourTile,
        TopRightNeighbourTile,
        BottomRightNeighbourTile
    };

    public Tile[] NeighbouringTiles => new[]
    {
        TopNeighbourTile,
        RightNeighbourTile,
        BottomNeighbourTile,
        LeftNeighbourTile
    };

    /*
     * Handling user hovering over tiles
     * Looks for a matched set
     * highlights any matched sets from the selected tile
     */
    public void OnPointerEnteredTile()
    {
        if (itemIsUnderElimination)
        {
            SetHighlight(false);
            return;
        }
        FindAllMatchingTiles();
        SetMatchedHighlight(true);
    }

    /*
     * Unhighlight matched sets
     */
    public void OnPointerExitedTile()
    {
        SetMatchedHighlight(false);
    }

    public bool IsTileSwappable()
    {
        return _item != null && _item.canBeSwapped;
    }

    public void OnPressed()
    {
        if (_terminableTiles.Count > 0)
            EliminateMatched();

        if (_swapEventEnabled)
            SwapJewels.Instance.OnPressed();
    }

    public void OnReleased()
    {
        if (_swapEventEnabled)
            SwapJewels.Instance.OnReleased();
    }

    /**
     * Checks and sets item to fall
     * Also checks if item is fragile - eliminates when about to fall
     */
    private async void ItemFallTry()
    {
        if (_item is null || !_item.canFall) return;

        List<Tile> path = _item.GetFallingPath(this);
        if (path.Count <= 0) return;
        if (_item.powerUp is Fragile)
        {
            await EliminationAction.Instance.CommenceEliminationAction(new List<Tile>() { this });
        }
        else
            await ItemFallEvent.Instance.CommenceFall(path);

        SetHighlight(false);
    }

    private List<Tile> GetTerminableRows()
    {
        var result = GetHorizontalRow();
        result.AddRange(GetVerticalRow());
        result = new List<Tile>(result).Distinct().ToList();
        return result;
    }

    private List<Tile> GetVerticalRow()
    {
        var fullRow = GetTilesInDirection(Direction.Top);
        fullRow.AddRange(GetTilesInDirection(Direction.Bottom));
        fullRow = new List<Tile>(fullRow).Distinct().ToList();
        return fullRow.Count > 2 ? fullRow : new List<Tile>();
    }

    private List<Tile> GetHorizontalRow()
    {
        var fullRow = GetTilesInDirection(Direction.Left);
        fullRow.AddRange(GetTilesInDirection(Direction.Right));
        fullRow = new List<Tile>(fullRow).Distinct().ToList();
        return fullRow.Count > 2 ? fullRow : new List<Tile>();
    }

    private List<Tile> GetTilesInDirection(Direction direction)
    {
        var result = new List<Tile> { this };
        var neighbourInDirection = direction switch
        {
            Direction.Top => TopNeighbourTile,
            Direction.Left => LeftNeighbourTile,
            Direction.Right => RightNeighbourTile,
            Direction.Bottom => BottomNeighbourTile,
            _ => null
        };

        if (neighbourInDirection is null) return result;
        if (neighbourInDirection.Item is not Jewel jewel) return result;
        if (jewel.jewelType == ((Jewel)Item).jewelType)
            result.AddRange(neighbourInDirection.GetTilesInDirection(direction));
        return result;
    }

    private void FindAllMatchingTiles()
    {
        if (_item is not Jewel) return;
        var rows = GetTerminableRows();
        var square = GetTilesFormingSquare();

        _terminableTiles = new List<Tile>(rows);
        _terminableTiles.AddRange(square);
        _terminableTiles = new List<Tile>(_terminableTiles).Distinct().ToList();
    }

    private void SetMatchedHighlight(bool highlight)
    {
        foreach (var tile in _terminableTiles)
        {
            tile.SetHighlight(highlight);
        }
    }

    private void SetHighlight(bool highlight)
    {
        GetComponent<Image>().color = highlight ? matchingTileHighlightColour : idleColour;
    }

    private List<Tile> GetTilesFormingSquare()
    {
        var validSquares = new List<Tile>();
        Tile[][] possibleSquares =
        {
            new[] { LeftNeighbourTile, TopLeftNeighbourTile, TopNeighbourTile },
            new[] { TopNeighbourTile, TopRightNeighbourTile, RightNeighbourTile },
            new[] { RightNeighbourTile, BottomRightNeighbourTile, BottomNeighbourTile },
            new[] { BottomNeighbourTile, BottomLeftNeighbourTile, LeftNeighbourTile }
        };

        foreach (var square in possibleSquares)
        {
            if (square.Any(item => item is null)) continue;
            if (!square.All(item => item.Item is Jewel jewel && jewel.jewelType == ((Jewel)_item).jewelType)) continue;
            validSquares.AddRange(square.ToList());
        }

        if (validSquares.Count <= 0) return validSquares;
        validSquares.Add(this);
        validSquares = new List<Tile>(validSquares).Distinct().ToList();
        return validSquares;
    }

    /**
     * Mainly created for handling the power-up sprites
     * but also swaps the jewel's sprite
     */
    private void SetupSprites(Item newItem)
    {
        if (newItem is not null)
        {
            if (newItem.sprite is not null)
                tileIcon.sprite = newItem.sprite;
            if (newItem.powerUp is not null)
            {
                _tileBackground.sprite = newItem.powerUp.sprite;
                _tileBackground.color = new Color(1, 1, 1, 1);
                return;
            }

            /*
             * Given the blocker cannot move,
             * if the item was 'turned' to a blocker,
             * it must have been an item with concretion.
             * Return to preserve the concretion sprite
             */
            if (newItem is Blocker) return;
        }

        _tileBackground.sprite = null;
        _tileBackground.color = new Color(1, 1, 1, 0);
    }
}