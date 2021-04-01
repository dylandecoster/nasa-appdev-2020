using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Terrain;

public class TerrainTester : MonoBehaviour
{
    public Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {

        

        // A 2 Dimentional Array of every single point on the terrain as is
        float[,] points = new float[513, 513];

        // Terrain counter
        float point = 0f;

        // A loop that fills up the array
        for(int i = 0; i < 513; i++)
        {
            for(int j = 0; j < 513; j++)
            {
                points[i, j] = point;
                point += 0.000001f;
            }
        }

        // Sets the terrain with all the points
        TerrainData terrainData = terrain.terrainData;
        terrainData.SetHeights(0, 0, points);
    }
}
