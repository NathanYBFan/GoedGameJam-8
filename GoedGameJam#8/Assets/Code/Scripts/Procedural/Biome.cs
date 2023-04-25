using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This class autotiles grass and tints it based on a perlin noise value mapped onto a colormap.
//This script should either be attached to a tilemap or be rewritten to include a tilemap reference.
public class Biome : MonoBehaviour
{
    [Header("Noise Settings")]       
    public int seed = -71;        
    public float scale = 1.0F;
    [Range(-1, 1)] public float waterThreshold = 0;    
    public float frequency = 0.01f;    
    public int octaves = 3;
    public float lacunarity = 2.0f;
    public float relativeNoiseGain = 0.5f;
    public FastNoise.FractalType fractalType = FastNoise.FractalType.FBM;


    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    [Header("Colour Maps")]
    public Texture2D gradMap; //Gradient colormap for biome shifting
    public Texture2D wGradMap; //Gradient colormap for water biome shifting
    

    [Header("TILES")]    
    public Grid grid;         //Reference to the grid of which this tilemap is a child of
    public RuleTile grass;        //The tiles we want to autotile. For this instance, it's grass tiles
    public Tile[] flowers;        //The tiles we want to autotile. For this instance, it's grass tiles
    public RuleTile sand;
    public RuleTile water;
    //public AnimatedTile water;        //The tile we want to autotile. For this instance, it's water

    
    public Vector3Int gridBottomLeft;//The bottom left corner of the viewport, but mapped to the cell grid of the tilemap.
    public Vector3Int gridTopRight;//The top right corner of the viewport, but mapped to the cell grid of the tilemap.
    public GameObject mask;   //Reference to a SpriteMask
    public GameObject player; //Reference to the player
    public int noiseValue;    //The noise value (but mapped to a 0 - 255 range)
    Vector3 size;             //The vector 3 scale of the entire viewport (viewable range)
    public int tempSeed = 1337;
    public int precSeed = 10273;
    void Start()
    {
        size = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        mask.transform.localScale = size;
        //voronoi.SetNoiseType(FastNoise.NoiseType.Cellular);
        //voronoi.SetFractalOctaves(2);
        GenerateTerrain();
    }

    void CalcNoise(int x, int y, bool isWater)
    {
        Vector3Int gridPos = grid.WorldToCell(new Vector3(x, y, 0));
        Color thisColor = new Color(255, 255, 255, 1);
        /*
        //This long mf line is mapping rgb to a pixel on the colourmap. Yertle flashbacks.
        if (!isWater)
        {
            thisColor = new Color(gradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).r, gradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).g, gradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).b);
        }
        else
        {
            thisColor = new Color(wGradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).r, wGradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).g, wGradMap.GetPixel(Mathf.Clamp(Mathf.CeilToInt(sample2 * temperatureNoise), 0, 255), Mathf.Clamp(Mathf.CeilToInt(sample2 * precipitationNoise), 0, 255)).b, 0.75f);

        }*/

        //You have to set the tile's flags to none before editing colours or else it won't work.
        GetComponent<Tilemap>().SetTileFlags(gridPos, TileFlags.None);
        GetComponent<Tilemap>().SetColor(gridPos, thisColor);

    }

/*
    void GenerateEdgeDetails()
    {
        gridBottomLeft = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)));
        gridTopRight = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)));

        //First iteration of terrain gen
        for (int y = gridBottomLeft.y; y <= gridTopRight.y; y++)
        {
            for (int x = gridBottomLeft.x; x <= gridTopRight.x; x++)
            {
                bool hasEdge = false;
                if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y, 0)) == grass[0])
                {

                    if (GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water)
                    {
                        //right edge is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[7]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                        hasEdge = true;
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //left edge is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[8]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                        hasEdge = true;
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water)
                    {
                        //top edge is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[13]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                        hasEdge = true;
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water)
                    {
                        //bottom edge is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[2]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                        hasEdge = true;
                    }

                    if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y + 1, 0)) == water && !hasEdge)
                    {
                        //Top corners is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[21]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y - 1, 0)) == water && !hasEdge)
                    {
                        //bottom corners is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[18]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y - 1, 0)) == water && !hasEdge)
                    {
                        //left corners is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[19]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y - 1, 0)) == water && !hasEdge)
                    {
                        //right corners is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[20]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }

                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y + 1, 0)) == water && !hasEdge)
                    {
                        //Top-right corner is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[12]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y + 1, 0)) == water && !hasEdge)
                    {
                        //Top-left corner is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[14]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y - 1, 0)) == water && !hasEdge)
                    {
                        //Bottom-left corner is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[3]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y - 1, 0)) == water && !hasEdge)
                    {
                        //Bottom-right corner is water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[1]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }

                    if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //all edges are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[10]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //all but top edge are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[5]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //all but bottom edge are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[16]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //all but right edge are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[11]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water)
                    {
                        //all but left edge are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[9]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water)
                    {
                        //bottom and right are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[4]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x + 1, y, 0)) == water)
                    {
                        //top and right are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[15]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y + 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //top and left are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[17]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }
                    else if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y - 1, 0)) == water && GetComponent<Tilemap>().GetTile(new Vector3Int(x - 1, y, 0)) == water)
                    {
                        //bottom and left are water
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass[6]);
                        CalcNoise(x, y, false);
                        if (sandLayer.GetTile(new Vector3Int(x, y, 0)) != sand) sandLayer.SetTile(new Vector3Int(x, y, 0), sand);
                    }

                }
            }
        }
    }
    */
    void GenerateTerrain()
    {
        FastNoise heightMap = new FastNoise();
        heightMap.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
        heightMap.SetSeed((int)seed);      
        heightMap.SetFrequency(frequency);     
        heightMap.SetFractalOctaves(octaves);     
        heightMap.SetFractalLacunarity(lacunarity);   
        heightMap.SetFractalGain(relativeNoiseGain);     
        heightMap.SetFractalType(fractalType);
        FastNoise defectorHeightMap = new FastNoise();
        defectorHeightMap.SetNoiseType(FastNoise.NoiseType.PerlinFractal);       
        gridBottomLeft = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)));
        gridTopRight = grid.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)));

        //First iteration of terrain gen
        for (int y = gridBottomLeft.y; y <= gridTopRight.y; y++)
        {
            for (int x = gridBottomLeft.x; x <= gridTopRight.x; x++)
            {
                //Grabbing the tile map and setting the tile at x, y to be grass.
                //DO NOTE: TileMap positions work in Vector3Int which DOES make a difference (Vector3 doesn't work).
                if (GetComponent<Tilemap>().GetTile(new Vector3Int(x, y, 0)) == null) {
                    if ((heightMap.GetPerlinFractal(x / scale, y / scale) + defectorHeightMap.GetPerlinFractal(x / scale, y / scale)) / 2 <= waterThreshold)
                    {
                        
                        //{
                            GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), water);
                        // CalcNoise(x, y, true);
                        //}
                    }
                    else
                    {
                        GetComponent<Tilemap>().SetTile(new Vector3Int(x, y, 0), grass);
                    // CalcNoise(x, y, false);
                    }
                    // EqualizeColours();
                }
            }
        }
    }
    void EqualizeColours()
    {
        float sumR = 0, sumG = 0, sumB = 0;
        float avgR, avgG, avgB;
        int counter = 0;
        for (int y = gridBottomLeft.y; y <= gridTopRight.y; y++)
        {
            for (int x = gridBottomLeft.x; x <= gridTopRight.x; x++)
            {
                Vector3Int gridPos = grid.WorldToCell(new Vector3(x, y, 0));
                Color thisColour = GetComponent<Tilemap>().GetColor(gridPos);
                sumR += thisColour.r;
                sumG += thisColour.g;
                sumB += thisColour.b;
                counter++;
            }
        }
        avgR = sumR / 255;
        avgG = sumG / 255;
        avgB = sumB / 255;

        for (int y = gridBottomLeft.y; y <= gridTopRight.y; y++)
        {
            for (int x = gridBottomLeft.x; x <= gridTopRight.x; x++)
            {
                Vector3Int gridPos = grid.WorldToCell(new Vector3(x, y, 0));
                Color thisColour = GetComponent<Tilemap>().GetColor(gridPos);
                Color newColour = thisColour * new Vector4(avgR, avgG, avgB, 1);

                GetComponent<Tilemap>().SetTileFlags(gridPos, TileFlags.None);
                GetComponent<Tilemap>().SetColor(gridPos, newColour);
            }
        }

    }
    void Update()
    {
        GenerateTerrain();
    }
}
