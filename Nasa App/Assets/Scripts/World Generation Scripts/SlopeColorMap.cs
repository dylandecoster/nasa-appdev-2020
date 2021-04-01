// this came from this website: https://forum.unity.com/threads/adding-terrain-textures-procedurally.374327/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static LayerData;

/*
 * This class applies terrain layers based on steepness.
 * Maximum Slope: 89.8422
 * Maximum Normalized Slope: 0.9982467
 */
public class SlopeColorMap
{
    private static float[,,] slopeMap; // an array that stores splatmap data
    private static LayerData[] listTextures; // stores how each color should appear (see the class LayerData for more info)
    private static int indexOfDefaultTexture; // Unity needs a default texture, so this stores which texture it is

    // get the splatmap from the file "SlopeSplatmap.txt"
    public static void ReadData()
    {
        // get the fully qualified path name for SlopeSplatmap.txt
        string fileName = Path.GetFullPath("SlopeSplatmap.txt");

        // if the file exists, read the splatmap data from it. Otherwise, create the data and place it in a new file
        if (File.Exists(fileName))
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            SlopeColorMap.slopeMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

            using (BinaryReader input = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < SlopeColorMap.slopeMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < SlopeColorMap.slopeMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < SlopeColorMap.slopeMap.GetUpperBound(2); k++)
                        {
                            SlopeColorMap.slopeMap[i, j, k] = input.ReadSingle();
                        }
                    }
                }
            }
        }
        else
        {
            SlopeColorMap.Create();

            using (BinaryWriter output = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 0; i < SlopeColorMap.slopeMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < SlopeColorMap.slopeMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < SlopeColorMap.slopeMap.GetUpperBound(2); k++)
                        {
                            output.Write(SlopeColorMap.slopeMap[i, j, k]);
                        }
                    }
                }
            }
        }
    }

    // apply the slope splatmap to the terrain
    public static void Draw()
    {
        Terrain.activeTerrain.terrainData.SetAlphamaps(0, 0, SlopeColorMap.slopeMap);
    }

    // create a new slope splatmap
    public static float[,,] Create()
    {
        // Get the active terrain data
        TerrainData terrainData = Terrain.activeTerrain.terrainData;

        // Get the terrain layers that will be used to paint the terrain
        listTextures = LayerData.GetSlopeLayerData(terrainData);

        // Your texture data (i.e. Splatmap) is stored internally as a 3d array of floats with x and y location as the first 2 dimensions of the array and the index of the texture to be used as the 3rd dimension
        SlopeColorMap.slopeMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        //This is just in case someone mixed up min and max when completing the inspector for this script
        for (int i = 0; i < listTextures.Length; i++)
        {
            if (listTextures[i].minSteepness > listTextures[i].maxSteepness)
            {
                float temp2 = listTextures[i].minSteepness;
                listTextures[i].minSteepness = listTextures[i].maxSteepness;
                listTextures[i].maxSteepness = temp2;
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


        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Calculate the normalized steepness of the terrain at this location
                float steepness = terrainData.GetSteepness(y_01, x_01);

                //Erase existing splatmap at this point
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    SlopeColorMap.slopeMap[x, y, i] = 0.0f;
                }

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                for (int i = 0; i < listTextures.Length; i++)
                {

                    //The rules you defined in the inspector are being applied for each texture
                    if (steepness >= listTextures[i].minSteepness && steepness <= listTextures[i].maxSteepness)
                    {
                        splatWeights[listTextures[i].index] = 1.0f;
                    }
                }

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                //If z is 0 at this location (i.e. no texture was applied), put default texture
                if (Mathf.Approximately(z, 0.0f))
                {
                    splatWeights[indexOfDefaultTexture] = 1.0f;
                }

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    SlopeColorMap.slopeMap[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the terrainData:
        return SlopeColorMap.slopeMap;
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
