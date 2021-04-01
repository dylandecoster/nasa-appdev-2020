// this came from this website: https://forum.unity.com/threads/adding-terrain-textures-procedurally.374327/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using static LayerData;

/*
 * This class applies only the moon surface texture to the moon.
 */
public class DefaultColorMap
{
    private static float[,,] defaultMap; // an array that stores splatmap data
    private static LayerData moonTexture; // stores how each color should appear (in this case, only the moon texture) (see the class LayerData for more info)
    private static int indexOfDefaultTexture; // Unity needs a default texture, so this stores which texture it is

    // get the splatmap from the file "DefaultSplatmap.txt"
    public static void ReadData()
    {
        // get the fully qualified path name for DefaultSplatmap.txt
        string fileName = Path.GetFullPath("DefaultSplatmap.txt");
        
        // if the file exists, read the splatmap data from it. Otherwise, create the data and place it in a new file
        if (File.Exists(fileName))
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            DefaultColorMap.defaultMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

            using (BinaryReader input = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < DefaultColorMap.defaultMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < DefaultColorMap.defaultMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < DefaultColorMap.defaultMap.GetUpperBound(2); k++)
                        {
                            DefaultColorMap.defaultMap[i, j, k] = input.ReadSingle();
                        }
                    }
                }
            }
        }
        else
        {
            DefaultColorMap.Create();

            using (BinaryWriter output = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                for (int i = 0; i < DefaultColorMap.defaultMap.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < DefaultColorMap.defaultMap.GetUpperBound(1); j++)
                    {
                        for (int k = 0; k < DefaultColorMap.defaultMap.GetUpperBound(2); k++)
                        {
                            output.Write(DefaultColorMap.defaultMap[i, j, k]);
                        }
                    }
                }
            }
        }
    }

    // apply the default splatmap to the terrain
    public static void Draw()
    {
        Terrain.activeTerrain.terrainData.SetAlphamaps(0, 0, DefaultColorMap.defaultMap);
    }

    // create a new default splatmap
    public static float[,,] Create()
    {
        // Get a reference to the terrain data
        TerrainData terrainData = Terrain.activeTerrain.terrainData;

        // Get the terrain layers that will be used to paint the terrain
        moonTexture = LayerData.GetDefaultLayerData(terrainData);

        // Your texture data (i.e. Splatmap) is stored internally as a 3d array of floats with x and y location as the first 2 dimensions of the array and the index of the texture to be used as the 3rd dimension
        DefaultColorMap.defaultMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        //This is just in case someone mixed up min and max when completing the inspector for this script
        if (moonTexture.minSteepness > moonTexture.maxSteepness)
        {
            float temp2 = moonTexture.minSteepness;
            moonTexture.minSteepness = moonTexture.maxSteepness;
            moonTexture.maxSteepness = temp2;
        }

        //For some reason you need a default texture in Unity
        indexOfDefaultTexture = moonTexture.index;

        // assign a layer (texture) to each point on the moon.
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Assign this point to the splatmap array
                DefaultColorMap.defaultMap[x, y, 0] = 1.0f;
            }
        }

        // Finally assign the new splatmap to the terrainData
        return DefaultColorMap.defaultMap;
    }
}
