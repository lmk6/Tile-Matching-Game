using System.Collections.Generic;
using System.Linq;
using ItemDB = ItemDatabase;
using PowerUpDB = PowerUpDatabase;

/*
 * Holds hard-coded levels and is being used
 * as the 'messenger' to send the chosen level
 * to the TileMatchingGame
 */
public static class LevelDatabase
{
    public static readonly List<Level> Levels;

    public static Level ChosenLevel;

    static LevelDatabase()
    {
        Levels = new List<Level> { Level1(), Level2(), Level3()};
        ItemDB.UnloadAssets();
        PowerUpDB.UnloadAssets();
    }
    

    public static Level  GetNextLevel()
    {
        var index = Levels.IndexOf(ChosenLevel);
        ChosenLevel = index == Levels.Count - 1 ? Levels.First() : Levels[index + 1];
        return ChosenLevel;
    }
    
    /*
     *  Levels are generated according to the array's initialisation layout
     */
    
    private static Level Level1()
    {
        var levelGridLayout = new[,]
        {
            {ItemDB.NewRedJewel(), ItemDB.NewYellowJewel(PowerUpDB.NewFragile()), ItemDB.NewSand(), ItemDB.NewGreenJewel(), ItemDB.NewBlueJewel(PowerUpDB.NewConcretion())},
            {ItemDB.NewRedJewel(), ItemDB.NewBlueJewel(), ItemDB.NewBlocker(), ItemDB.NewBlocker(), ItemDB.NewBlueJewel(PowerUpDB.NewConcretion())},
            {ItemDB.NewBlueJewel(), ItemDB.NewBlueJewel(), ItemDB.NewPurpleJewel(), ItemDB.NewPurpleJewel(), ItemDB.NewBlueJewel(PowerUpDB.NewConcretion())},
            {ItemDB.NewRedJewel(PowerUpDB.NewBomb()), ItemDB.NewYellowJewel(), ItemDB.NewPurpleJewel(), ItemDB.NewGreenJewel(), ItemDB.NewPurpleJewel()},
            {ItemDB.NewBlocker(), ItemDB.NewGreenJewel(), ItemDB.NewGreenJewel(), ItemDB.NewPurpleJewel(), ItemDB.NewGreenJewel(PowerUpDB.NewColourBomb())},
        };

        const string name = "Level - Easy";
        const int swaps = 4;

        var level = new Level(name, swaps, levelGridLayout);
        
        return level;
    }

    private static Level Level2()
    {
        var levelGridLayout = new[,]
        {
            {ItemDB.NewRedJewel(), ItemDB.NewRedJewel(), ItemDB.NewSand(), ItemDB.NewPurpleJewel(PowerUpDB.NewFragile()), ItemDB.NewYellowJewel()},
            {ItemDB.NewGreenJewel(), ItemDB.NewSand(), ItemDB.NewGreenJewel(PowerUpDB.NewConcretion()), ItemDB.NewGreenJewel(), ItemDB.NewRedJewel(PowerUpDB.NewFragile())},
            {ItemDB.NewRedJewel(), ItemDB.NewSand(), ItemDB.NewBlocker(), ItemDB.NewPurpleJewel(), ItemDB.NewYellowJewel()},
            {ItemDB.NewBlocker(), ItemDB.NewPurpleJewel(), ItemDB.NewBlueJewel(PowerUpDB.NewConcretion()), ItemDB.NewYellowJewel(), ItemDB.NewYellowJewel(PowerUpDB.NewBomb())},
            {ItemDB.NewBlueJewel(PowerUpDB.NewBomb()), ItemDB.NewPurpleJewel(), ItemDB.NewYellowJewel(PowerUpDB.NewFragile()), ItemDB.NewBlueJewel(PowerUpDB.NewConcretion()), ItemDB.NewBlocker()},
        };

        const string name = "Level - Hard";
        const int swaps = 5;

        var level = new Level(name, swaps, levelGridLayout);
        
        return level;
    }

    private static Level Level3()
    {
        var levelGridLayout = new[,]
        {
            {ItemDB.NewBlocker(), ItemDB.NewBlocker(), ItemDB.NewRedJewel(), ItemDB.NewBlocker(), ItemDB.NewBlocker()},
            {ItemDB.NewRedJewel(), ItemDB.NewRedJewel(), ItemDB.NewSand(), ItemDB.NewRedJewel(), ItemDB.NewRedJewel()},
            {ItemDB.NewBlueJewel(PowerUpDB.NewConcretion()), ItemDB.NewBlocker(), ItemDB.NewYellowJewel(), ItemDB.NewBlocker(), ItemDB.NewGreenJewel(PowerUpDB.NewConcretion())},
            {ItemDB.NewYellowJewel(), ItemDB.NewBlueJewel(), ItemDB.NewBlocker(), ItemDB.NewGreenJewel(), ItemDB.NewYellowJewel()},
            {ItemDB.NewBlueJewel(), ItemDB.NewBlueJewel(PowerUpDB.NewFragile()), ItemDB.NewYellowJewel(PowerUpDB.NewColourBomb()), ItemDB.NewGreenJewel(PowerUpDB.NewFragile()), ItemDB.NewGreenJewel()},
        };

        const string name = "Colour madness";
        const int swaps = 4;

        var level = new Level(name, swaps, levelGridLayout);
        
        return level;
    }
}