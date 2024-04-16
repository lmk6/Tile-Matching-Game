using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Items;
using UnityEngine;

/**
 * Initialises the grid and populates it with the chosen level's items layout
 * Controls the UI - Tiles' buttons
 */
public sealed class BoardController : MonoBehaviour
{
    public static BoardController Instance { get; private set; }
    public Row[] rows;
    public Tile[,] GridOfTiles { get; private set; }
    private List<Tile> _tiles;

    public int BoardWidth()
    {
        return GridOfTiles.GetLength(0);
    }

    public int BoardHeight()
    {
        return GridOfTiles.GetLength(1);
    }

    private void Awake()
    {
        Instance = this;
    }

    public void BoardInitialization(Item[,] gridItems)
    {
        GridOfTiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];
        _tiles = new List<Tile>();

        for (var x = 0; x < BoardWidth(); x++)
        {
            for (var y = 0; y < BoardHeight(); y++)
            {
                var tile = rows[y].tiles[x];
                tile.x = x;
                tile.y = y;
                tile.Item = gridItems[y, x];
                GridOfTiles[x, y] = tile;
                _tiles.Add(tile);
            }
        }
    }

    public int GetNumOfJewelsLeft()
    {
        var jewelCount = 0;

        _tiles.ForEach(tile => 
            jewelCount = tile.Item is not null && tile.Item is Jewel ?
            ++jewelCount :
            jewelCount);

        return jewelCount;
    }

    public bool AtLeastOneMatchingSet()
    {
        return _tiles.Any(tile => tile.HasMatchingSet());
    }

    /**
     * Designed to prevent the player from interacting with board while animation is still running
     */
    public async Task DisableInteraction()
    {
        await SwitchEnableAllButtons(false);
    }

    public async Task EnableInteraction()
    {
        await SwitchEnableAllButtons(true);
    }

    private Task SwitchEnableAllButtons(bool enable)
    {
        foreach (var tile in GridOfTiles)
        {
            if (tile.Item is not Jewel) continue;
            tile.button.enabled = enable;
            tile.SwapEventEnabled = enable;
        }

        return Task.CompletedTask;
    }
}