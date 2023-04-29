using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum TileTypes {
        machines,
        environment
    }

    public enum EnvironmentTileType 
    {
        Ground,
        Grass,
        Wheat,
        Flower,
        Water,
        Soil
    }
    public enum ItemType {
        Dirt,
        Ash,
        Coal,
        Flower,
        FlowerSeeds,
        Grass,
        GrassSeeds,
        Soil,
        TreeSeeds,
        Water,
        Wheat,
        WheatSeeds,
        Wood,
        Unknown

    }
    public enum Directions {
        up = 0,
        upRight = 1,
        rightUp = 2,
        right = 3,
        rightDown = 4,
        downRight = 5,
        down = 6,
        downLeft = 7,
        leftDown = 8,
        left = 9,
        leftUp = 10,
        upLeft = 11
    }

}
