using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Terrain;
using static TerrainConverter;

public class CWG : MonoBehaviour
{
    public int start = 0; // Customizes where the reader starts
    public int end = 7223208; // Customizes where the reader ends

    public Terrain terrainX1Y1; // Gets the map data
    public Terrain terrainX1Y2; // Quad 1
    public Terrain terrainX2Y1; 
    public Terrain terrainX2Y2;
    public Terrain terrainXN1Y1; //Quad 2
    public Terrain terrainXN1Y2;
    public Terrain terrainXN2Y1;
    public Terrain terrainXN2Y2;
    public Terrain terrainXN1YN1; // Qaud 3
    public Terrain terrainXN1YN2;
    public Terrain terrainXN2YN1;
    public Terrain terrainXN2YN2;
    public Terrain terrainX1YN1; // Quad 4 
    public Terrain terrainX1YN2;
    public Terrain terrainX2YN1;
    public Terrain terrainX2YN2;

    double[] lat , lon, height, slope; // Stores all the columns data
    void Start()
    {
        // Sets up the arrays
        int lines = end - start;
        lat = new double[lines];
        lon = new double[lines];
        height = new double[lines];
        slope = new double[lines];

        // Fills the arrays
        ReadCoordinates(lines);

        // Creates terrain
        // Quad 1
        CreateTerrain(terrainX1Y1, 0, 0, 1);
        CreateTerrain(terrainX1Y2, 0, 514, 1);
        CreateTerrain(terrainX2Y1, 514, 0, 1);
        CreateTerrain(terrainX2Y2, 514, 514, 1);

        // Quad 2
        CreateTerrain(terrainXN1Y1, -514, 0, 2);
        CreateTerrain(terrainXN1Y2, -514, 514, 2);
        CreateTerrain(terrainXN2Y1, -1028, 0, 2);
        CreateTerrain(terrainXN2Y2, -1028, 514, 2);

        // Quad 3
        CreateTerrain(terrainXN1YN1, -514, -514, 3);
        CreateTerrain(terrainXN1YN2, -514, -1028, 3);
        CreateTerrain(terrainXN2YN1, -1028, -514, 3);
        CreateTerrain(terrainXN2YN2, -1028, -1028, 3);

        // Quad 4
        CreateTerrain(terrainX1YN1, 0, -514, 4);
        CreateTerrain(terrainX1YN2, 0, -1028, 4);
        CreateTerrain(terrainX2YN1, 514, -514, 4);
        CreateTerrain(terrainX2YN2, 514, -1028, 4);
    }
    
    void ReadCoordinates(int lines)
    {
        StreamReader reader = new StreamReader("Assets/Lunar Coordinates/fy20_adc_data_file_88_degrees.csv"); // File to read from

        
        string str; // Temporarily stores the line it reads
        string[] strArray; // Stores each part of the line

        // Makes the reader go down to desired line
        for(int i = 0; i < start; i++)
        {
            reader.ReadLine();
        }

        for(int i = 0; i < lines; i++)
        {
            str = reader.ReadLine(); // Stores the next line to the string
            strArray = str.Split(','); // Splits the line into seperate words

            // Stores each of the data to its corresponding array
            lat[i] = double.Parse(strArray[0]);
            lon[i] = double.Parse(strArray[1]);
            height[i] = double.Parse(strArray[2]);
            slope[i] = double.Parse(strArray[3]);
        }
    }

    void CreateTerrain(Terrain terrain, int xmin, int ymin, int quadrant)
    {
        TerrainConverter converter;

        //A 2 Dimentional Array of every single point on the terrain as is
        float[,] points = new float[513, 513];

        // A loop that fills up the array
        for (int i = start; i < end; i++)
        {
            switch(quadrant){
                case 4:
                    if (lon[i] <= 90.0 && lon[i] >= 0.0)
                    {
                        converter = new TerrainConverter(lat[i], lon[i]);
                        if (converter.x >= xmin && converter.x <= xmin + 513 && converter.z >= ymin && converter.z <= ymin + 513)
                        {
                            points = CoordinateToPoint(points, xmin, ymin, lat[i], lon[i], height[i]);
                        }
                    }
                    break;
                case 3:
                    if (lon[i] <= 180.0 && lon[i] >= 90.0)
                    {
                        converter = new TerrainConverter(lat[i], lon[i]);
                        if (converter.x >= xmin && converter.x <= xmin + 513 && converter.z >= ymin && converter.z <= ymin + 513)
                        {                          
                            points = CoordinateToPoint(points, xmin, ymin, lat[i], lon[i], height[i]);
                        }
                    }
                    break;
                case 2:
                    if (lon[i] >= -180.0 && lon[i] <= -90.0)
                    {
                        converter = new TerrainConverter(lat[i], lon[i]);
                        if (converter.x >= xmin && converter.x <= xmin + 513 && converter.z >= ymin && converter.z <= ymin + 513)
                        {
                            points = CoordinateToPoint(points, xmin, ymin, lat[i], lon[i], height[i]);
                        }
                    }
                    break;
                case 1:
                    if (lon[i] >= -90.0 && lon[i] <= 0.0)
                    {
                        converter = new TerrainConverter(lat[i], lon[i]);
                        if (converter.x >= xmin && converter.x <= xmin + 513 && converter.z >= ymin && converter.z <= ymin + 513)
                        {
                            points = CoordinateToPoint(points, xmin, ymin, lat[i], lon[i], height[i]);
                        }
                    }
                    break;
            }
        }


        points = PointCleaner(points);


        // Sets the terrain with all the points
        TerrainData terrainData = terrain.terrainData;
        terrainData.SetHeights(0, 0, points);
    }

    float[,] CoordinateToPoint(float[,] points, double XOffset, double YOffset, double lat, double lon, double height)
    {
        TerrainConverter converter = new TerrainConverter(lat, lon);

        int x = (int)(converter.x - XOffset);
        int y = (int)(converter.z - YOffset);


        float point = (float)(0.5 + .0001 * height);

        if(point > 1f)
        {
            point = 1f;
        }
        if(point < 0f)
        {
            point = 0f;
        }

        try
        {
            points[y, x] = point;
        }
        catch(System.IndexOutOfRangeException e)
        {
           // Debug.Log(x + " " + y);
        }

        return points;
    }

    // Due to some rounding errors there were random points missed, this cleans it up
    float[,] PointCleaner(float[,] points)
    {
        for(int i = 0; i < 513; i++)
        {
            for (int j = 0; j < 513; j++)
            {
                if (points[i, j] == 0 && j < 512)
                {
                    if (points[i, j + 1] != 0)
                    {
                        points[i, j] = points[i, j + 1];

                    }
                }

                else if(points[i, j] == 0)
                {
                    points[i, j] = points[i, j - 1];
                }
            }
        }

        return points;

    }

}
