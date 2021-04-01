// this came from this website: https://forum.unity.com/threads/adding-terrain-textures-procedurally.374327/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using static LayerData;

/*
 * This class applies terrain layers based on height.
 * Maximum Height: 1000
 * Maximum Normalized Height: 1 (It's always 1)
 */
public class HeightColorMap
{
    private static float[,,] heightMap; // an array that stores splatmap data
    private static LayerData[] listTextures; // stores how each color should appear (see the class LayerData for more info)
    private static int indexOfDefaultTexture; // Unity needs a default texture, so this stores which texture it is

    // get the splatmap from the file "HeightSplatmap.txt"
    public static void ReadData()
    {
        // get the fully qualified path name for HeightSplatmap.txt
        string fileName = Path.GetFullPath("HeightSplatmap.txt");

        // if the file exists, read the splatmap data from it. Otherwise, create the data and place it in a new file
        if (File.Exists(fileName))
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            HeightColorMap.heightMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

            using (BinaryReader input = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < HeightColorMap.heightMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < HeightColorMap.heightMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < HeightColorMap.heightMap.GetUpperBound(2); k++)
                        {
                            HeightColorMap.heightMap[i, j, k] = input.ReadSingle();
                        }
                    }
                }
            }
        }
        else
        {
            HeightColorMap.Create();

            using (BinaryWriter output = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 0; i < HeightColorMap.heightMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < HeightColorMap.heightMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < HeightColorMap.heightMap.GetUpperBound(2); k++)
                        {
                            output.Write(HeightColorMap.heightMap[i, j, k]);
                        }
                    }
                }
            }
        }
    }

    // apply the height splatmap to the terrain
    public static void Draw()
    {
        Terrain.activeTerrain.terrainData.SetAlphamaps(0, 0, HeightColorMap.heightMap);
    }

    // create a new height splatmap
    public static void Create()
    {
        // Get the active terrain data
        TerrainData terrainData = Terrain.activeTerrain.terrainData;

        // Get the terrain layers that will be used to paint the terrain
        listTextures = LayerData.GetHeightLayerData(terrainData);

        //See below for the definition of GetMaxHeight
        float maxHeight = 1000f;
        //float maxHeight = GetMaxHeight(terrainData, terrainData.heightmapWidth);

        // Your texture data (i.e. Splatmap) is stored internally as a 3d array of floats with x and y location as the first 2 dimensions of the array and the index of the texture to be used as the 3rd dimension
        HeightColorMap.heightMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        //This is just in case someone mixed up min and max when completing the inspector for this script
        for (int i = 0; i < listTextures.Length; i++)
        {
            if (listTextures[i].minAltitude > listTextures[i].maxAltitude)
            {
                float temp = listTextures[i].minAltitude;
                listTextures[i].minAltitude = listTextures[i].maxAltitude;
                listTextures[i].maxAltitude = temp;
            }
        }

        //For some reason you need a default texture in Unity
        for (int i = 0; i < listTextures.Length; i++)
        {
            if (listTextures[i].defaultTexture)
            {
                indexOfDefaultTexture = listTextures[i].index;
            }
        }

        // apply the layers
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Calculate the normalized height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float normHeight = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth)) / maxHeight;

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                for (int i = 0; i < listTextures.Length; i++)
                {

                    //The rules you defined in the inspector are being applied for each texture
                    if (normHeight >= listTextures[i].minAltitude && normHeight <= listTextures[i].maxAltitude)
                    {
                        splatWeights[listTextures[i].index] = 1.0f;
                    }
                }

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    HeightColorMap.heightMap[x, y, i] = splatWeights[i];
                }
            }
        }
    }

    //This is to get the maximum altitude of your terrain. For some reason TerrainData.
    public static float GetMaxHeight(TerrainData tData, int heightmapWidth)
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
}
