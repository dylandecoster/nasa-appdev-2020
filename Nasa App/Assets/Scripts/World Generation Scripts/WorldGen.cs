using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.Terrain;
using static CoordinateConverter;

public class WorldGen : MonoBehaviour
{
    //const int lines = 7223208 -- Total number of lines in the file
    int lines = 7223208; // How many lines to read
    
    const double maxHeight = 1958, minHeight = -4249.5; // The maximum and minimum heights of the file

    public Terrain terrainX1Y1;

    double[] lat, lon, height, slope; // Stores all the columns data
    void Start()
    {
        // Sets up the arrays
        lat = new double[lines];
        lon = new double[lines];
        height = new double[lines];
        slope = new double[lines];

        ReadCoordinates();
        CreateTerrain(terrainX1Y1);

    }

    void ReadCoordinates()
    {
        StreamReader reader = new StreamReader("Assets/Lunar Coordinates/fy20_adc_data_file_88_degrees.csv"); // File to read from

        string str; // Temporarily stores the line it reads
        string[] strArray; // Stores each part of the line

        for (int i = 0; i < lines; i++)
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

        void CreateTerrain(Terrain terrain)
        {
            bool repeat = false;

            //A 2 Dimentional Array of every single point on the terrain as is
            float[,] points = new float[4097, 4097];

            // A loop that fills up the array
            for (int i = 0; i < lines; i++)
            {
                points = CoordinateToPoint(points, 2048, 2048, lat[i], lon[i], height[i]);
            }

            points = PointCleaner(points);


            // Sets the terrain with all the points
            TerrainData terrainData = terrain.terrainData;
            terrainData.SetHeights(0, 0, points);
        }

        float[,] CoordinateToPoint(float[,] points, double XOffset, double ZOffset, double lat, double lon, double height)
        {
            CoordinateConverter converter = new CoordinateConverter(lat, lon, height);

            int x = (int)(converter.x + XOffset);
            int z = (int)(converter.z + ZOffset);

            float point = (float)(0.75 +  0.00013 * converter.y);

            if (point > 1f)
            {
                point = 1f;
            }
            if (point < 0f)
            {
                point = 0f;
            }

            try
            {
                points[z, x] = point;

                if (x < 4096 && points[z, x + 1] == 0)
                {
                    points[z, x + 1] = point;
                }

                if (x > 0 && points[z, x - 1] == 0)
                {
                    points[z, x - 1] = point;
                }

                if (z < 4096 && points[z + 1, x] == 0)
                {
                    points[z + 1, x] = point;

                }

                if (z > 0 && points[z - 1, x] == 0)
                {
                    points[z - 1, x] = point;

                }

        }
            catch (System.IndexOutOfRangeException e)
            {
               // print(e);
            }

            return points;
        }

        // Due to some rounding errors there were random points missed, this cleans it up
        float[,] PointCleaner(float[,] points)
        {
            for (int i = 0; i < 4097; i++)
            {
                for (int j = 0; j < 4097; j++)
                {
                    if (points[i, j] == 0 && j < 4096)
                    {
                        if (points[i, j + 1] != 0)
                        {
                            points[i, j] = points[i, j + 1];

                        }
                    }

                    else if (points[i, j] == 0 && j > 0)
                    {
                        if (points[i, j - 1] != 0)
                        {
                            points[i, j] = points[i, j - 1];

                        }
                    }

                    else if (points[i, j] == 0 && i < 4096)
                    {
                        if (points[i + 1, j] != 0)
                        {
                            points[i, j] = points[i + 1, j];

                        }
                     }

                    else if (points[i, j] == 0 && i > 0)
                    {
                        if (points[i - 1, j] != 0)
                        {
                            points[i, j] = points[i - 1, j];

                        }
                    }
                }
            }

            for (int i = 1; i < 4096; i++)
            {
                for (int j = 1; j < 4096; j++)
                {
                    var a = points[i - 1, j];
                    var b = points[i, j - 1];
                    var c = points[i + 1, j];
                    var d = points[i, j + 1];

                    var e = a + b + c + d;
                    points[i, j] = e / 4;
                }
            }

                return points;

        }

}

