using Power_Ups;
using UnityEngine;

/*
 * Loads and instantiates Power Ups
 * Gives a method to Unload assets to free the memory
 */
public static class PowerUpDatabase
{
    private static PowerUp _bombAsset;
    private static PowerUp _colourBombAsset;
    private static PowerUp _fragileAsset;
    private static PowerUp _concretionAsset;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialise()
    {
        _bombAsset = LoadPowerUpAsset("Bomb");
        _colourBombAsset = LoadPowerUpAsset("ColourBomb");
        _fragileAsset = LoadPowerUpAsset("Fragile");
        _concretionAsset = LoadPowerUpAsset("Concretion");
    }
    
    public static PowerUp NewBomb()
    {
        return Object.Instantiate(_bombAsset);
    }

    public static PowerUp NewColourBomb()
    {
        return Object.Instantiate(_colourBombAsset);
    }

    public static PowerUp NewFragile()
    {
        return Object.Instantiate(_fragileAsset);
    }

    public static PowerUp NewConcretion()
    {
        return Object.Instantiate(_concretionAsset);
    }
    
    private static PowerUp LoadPowerUpAsset(string assetName)
    {
        var asset = Resources.Load<PowerUp>($"Power Ups/{assetName}");
        return asset;
    }

    public static void UnloadAssets()
    {
        Resources.UnloadAsset(_bombAsset);
        Resources.UnloadAsset(_colourBombAsset);
        Resources.UnloadAsset(_fragileAsset);
        Resources.UnloadAsset(_concretionAsset);
    } 
}