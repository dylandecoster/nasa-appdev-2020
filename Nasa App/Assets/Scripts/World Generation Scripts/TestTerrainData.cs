using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestTerrainData : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;
    private int indexOfDefaultTexture;

    void Start()
    {

        // Get the attached terrain component
        terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        terrainData = terrain.terrainData;

        //See below for the definition of GetMaxHeight
        float maxHeight = GetMaxHeight(terrainData, terrainData.heightmapWidth);
        float maxSlope = GetMaxSlope(terrainData);

        Debug.Log("Max Height: " + maxHeight);
        Debug.Log("Max Height heightmap: " + terrainData.heightmapHeight);
        Debug.Log("Max Height alphamap: " + terrainData.alphamapHeight);

        Debug.Log("Max Slope: " + maxSlope);
        Debug.Log("Max Normalized Height: 1");
        Debug.Log("Max Normalized Slope: " + maxSlope / 90.0f);
    }

    //This is to get the maximum height of your terrain. For some reason TerrainData.
    private float GetMaxHeight(TerrainData tData, int heightmapWidth)
    {

        float maxHeight = 0f;

        for (int x = 0; x < heightmapWidth; x++)
        {
            for (int y = 0; y < heightmapWidth; y++)
            {
                if (tData.GetHeight(x, y) > maxHeight)
                {
                    maxHeight = tData.GetHeight(x, y);
                }
            }
        }
        return maxHeight;
    }

    //This is to get the maximum slope of your terrain. For some reason TerrainData.
    private float GetMaxSlope(TerrainData tData)
    {

        float maxSlope = 0f;

        for (int y = 0; y < tData.alphamapHeight; y++)
        {
            for (int x = 0; x < tData.alphamapWidth; x++)
            {
                float normY = (float)y / (float)tData.alphamapHeight;
                float normX = (float)x / (float)tData.alphamapWidth;

                if (tData.GetSteepness(normY, normX) > maxSlope)
                {
                    maxSlope = tData.GetSteepness(normY, normX);
                }
            }
        }
        return maxSlope;
    }
}