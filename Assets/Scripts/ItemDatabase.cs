using Items;
using Power_Ups;
using UnityEngine;
using Object = UnityEngine.Object;

/*
 * Loads and instantiates Items
 * Gives a method to Unload assets to free the memory
 */
public static class ItemDatabase
{
    private static Item _redJewelAsset;
    private static Item _blueJewelAsset;
    private static Item _greenJewelAsset;
    private static Item _yellowJewelAsset;
    private static Item _purpleJewelAsset;
    private static Item _sandAsset;
    private static Item _blockerAsset;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialise()
    {
        _redJewelAsset = LoadItemAsset("Red Jewel");
        _blueJewelAsset = LoadItemAsset("Blue Jewel");
        _greenJewelAsset = LoadItemAsset("Green Jewel");
        _yellowJewelAsset = LoadItemAsset("Yellow Jewel");
        _purpleJewelAsset = LoadItemAsset("Purple Jewel");
        _sandAsset = LoadItemAsset("Sand");
        _blockerAsset = LoadItemAsset("Rock_1");
    }

    public static Item NewRedJewel(PowerUp powerUp = null)
    {
        var newJewel = Object.Instantiate(_redJewelAsset);
        if (powerUp is not null)
        {
            newJewel.powerUp = powerUp;
        }

        return newJewel;
    }

    public static Item NewBlueJewel(PowerUp powerUp = null)
    {
        var newJewel = Object.Instantiate(_blueJewelAsset);
        if (powerUp is not null)
        {
            newJewel.powerUp = powerUp;
        }

        return newJewel;
    }

    public static Item NewGreenJewel(PowerUp powerUp = null)
    {
        var newJewel = Object.Instantiate(_greenJewelAsset);
        if (powerUp is not null)
        {
            newJewel.powerUp = powerUp;
        }

        return newJewel;
    }

    public static Item NewPurpleJewel(PowerUp powerUp = null)
    {
        var newJewel = Object.Instantiate(_purpleJewelAsset);
        if (powerUp is not null)
        {
            newJewel.powerUp = powerUp;
        }

        return newJewel;
    }

    public static Item NewYellowJewel(PowerUp powerUp = null)
    {
        var newJewel = Object.Instantiate(_yellowJewelAsset);
        if (powerUp is not null)
        {
            newJewel.powerUp = powerUp;
        }

        return newJewel;
    }

    public static Item NewBlocker()
    {
        return Object.Instantiate(_blockerAsset);
    }

    public static Item NewSand()
    {
        return Object.Instantiate(_sandAsset);
    }

    private static Item LoadItemAsset(string assetName)
    {
        var asset = Resources.Load<Item>($"Items/{assetName}");
        return asset;
    }

    public static void UnloadAssets()
    {
        Resources.UnloadAsset(_redJewelAsset);
        Resources.UnloadAsset(_blueJewelAsset);
        Resources.UnloadAsset(_greenJewelAsset);
        Resources.UnloadAsset(_yellowJewelAsset);
        Resources.UnloadAsset(_purpleJewelAsset);
        Resources.UnloadAsset(_sandAsset);
        Resources.UnloadAsset(_blockerAsset);
    }
}