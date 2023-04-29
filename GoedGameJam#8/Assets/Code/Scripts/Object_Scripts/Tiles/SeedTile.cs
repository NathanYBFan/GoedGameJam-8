using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Seed Tile")]
public class SeedTile : Tile
{
    public int randSeed;
    public int seedsAmount;
    
  
    public void Initialize(int randNum, int amount)
    {
        randSeed = randNum;
        seedsAmount = amount;
    }


}
