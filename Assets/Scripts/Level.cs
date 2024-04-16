using Items;

public class Level
{
    public readonly Item[,] LevelGridLayout;
    public readonly string LevelName;
    public readonly int NumOfSwaps;

    public Level(string name, int swaps, Item[,] gridLayout)
    {
        LevelName = name;
        NumOfSwaps = swaps;
        LevelGridLayout = gridLayout;
    }
}