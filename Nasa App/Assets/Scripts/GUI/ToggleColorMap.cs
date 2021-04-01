using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DefaultColorMap;
using static HeightColorMap;
using static SlopeColorMap;

public class ToggleColorMap: MonoBehaviour
{
    private Terrain t;
    int i = 1;

    void Start()
    {
        DefaultColorMap.ReadData(); // Sets the map at the begining to just look like the moon
        HeightColorMap.ReadData();
        SlopeColorMap.ReadData();
    }

    public void OnClick()
    {
        // Cycles through the different color maps
        if (i == 0)
        {
            DefaultColorMap.Draw(); // Default
            i = 1;
        }

        else if (i == 1)
        {
            HeightColorMap.Draw(); // Height
            i = 2;
        }

        else
        {
            SlopeColorMap.Draw(); // Slope
            i = 0;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            DefaultColorMap.Draw();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            HeightColorMap.Draw();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SlopeColorMap.Draw();
        }
    }
}
