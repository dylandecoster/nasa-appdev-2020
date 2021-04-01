using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class defines where textures appear based on height and slope.
 * 
 * Maximum Height: 1000
 * Maximum Normalized Height: 1 (It's always 1)
 * 
 * Maximum Slope: 89.8422
 * Maximum Normalized Slope: 0.9982467
 * 
 * The terrain layers are added to the moon in this order:
 * Moon surface texture:
 * Light Gray
 * 
 * Slope layers:
 * Orange
 * Yellow
 * Green
 * Blue
 * 
 * Height layers:
 * Pink
 * Purple
 * Green
 * Yellow
 * Red
 * 
 * Invalid layers:
 * Black
 * 
 */
public class LayerData
{
    public const float MIN_MOON_HEIGHT = 190f;
    public const float NORM_MIN_MOON_HEIGHT = 0.19f;
    public string name;
    public int index;
    public bool defaultTexture;
    public float minSteepness;
    public float maxSteepness;
    public float minAltitude;
    public float maxAltitude;

    public LayerData(string layerName, int layerIndex, bool isDefault = false, float minSlope = 0f, float maxSlope = 90.0f, float minHeight = 0f, float maxHeight = 1.0f)
    {
        this.name = layerName;
        this.index = layerIndex;
        this.defaultTexture = isDefault;
        this.minSteepness = minSlope;
        this.maxSteepness = maxSlope;
        this.minAltitude = minHeight;
        this.maxAltitude = maxHeight;
    }

    // applies only the moon surface texture to the moon ground
    public static LayerData GetDefaultLayerData(TerrainData terrainData)
    {
        // the terrain layers to use.
        LayerData listTextures = new LayerData("Light Gray - Moon Surface", 0, isDefault: true, minHeight: 0.0f, maxHeight: 1.0f);
        return listTextures;
    }

    /*
     * This applies the color coded height map to the terrain.
     * The following textures are applied from bottom to top (measurements are in meters and are relative to the terrain height, not the moon's height):
     * 
     * Pink (190m - 352m)
     * Purple (352m - 514m)
     * Green (514m - 676m)
     * Yellow (676m - 838m)
     * Red (838m - 1000m)
     */
    public static LayerData[] GetHeightLayerData(TerrainData terrainData)
    {
        // the terrain layers to use.
        LayerData[] listTextures = new LayerData[5];

        // the minimum and maximum height of the terrain
        float minMoonHeight = LayerData.MIN_MOON_HEIGHT; // 190m
        float normMinMoonHeight = LayerData.NORM_MIN_MOON_HEIGHT; // 0.19
        float maxTerrainHeight = 1000f;

        // these define the bounds of where each texture appears
        // note that the boundaries of each texture are normalized
        // for example, 0.19 is 19% of the terrain height.

        // the height of the terrain that's actually part of the moon aka not out of bounds.
        float maxMoonHeight = 810f; // 810m

        // the normalized size of each terrain layer, minus the out of bounds layer (black).
        float layerSize = maxMoonHeight / listTextures.Length / maxTerrainHeight; // 0.162


        // first (lowest) layer (pink)
        float pinkLayerMinHeight = normMinMoonHeight; // 0.19 (190m)
        float pinkLayerMaxHeight = pinkLayerMinHeight + layerSize; // 0.352 (352m)
        //float pinkLayerMaxHeight = 0.344f;

        // second layer (purple)
        float purpleLayerMinHeight = pinkLayerMaxHeight; // 0.352 (352m)
        float purpleLayerMaxHeight = purpleLayerMinHeight + layerSize; // 0.514 (514m)
        //float purpleLayerMaxHeight = 0.508f;

        // third layer (green)
        float greenLayerMinHeight = purpleLayerMaxHeight; // 0.514 (514m)
        float greenLayerMaxHeight = greenLayerMinHeight + layerSize; // 0.676 (676m)
        //float greenLayerMaxHeight = 0.672f;

        // fourth layer (yellow)
        float yellowLayerMinHeight = greenLayerMaxHeight; // 0.676 (676m)
        float yellowLayerMaxHeight = yellowLayerMinHeight + layerSize; // 0.838 (838m)
        //float yellowLayerMaxHeight = 0.836f;

        // fifth (highest) layer (red)
        float redLayerMinHeight = yellowLayerMaxHeight; // 0.838 (838m)
        float redLayerMaxHeight = redLayerMinHeight + layerSize; // 1 (1000m)
        //float redLayerMaxHeight = 1;

        // fill the listTextures array with height color data
        listTextures[0] = new LayerData("Pink - Bottom", 5, minHeight: pinkLayerMinHeight, maxHeight: pinkLayerMaxHeight);
        listTextures[1] = new LayerData("Purple - Intermediate", 6, minHeight: purpleLayerMinHeight, maxHeight: purpleLayerMaxHeight);
        listTextures[2] = new LayerData("Green - Middle", 7, minHeight: greenLayerMinHeight, maxHeight: greenLayerMaxHeight);
        listTextures[3] = new LayerData("Yellow - Intermediate", 8, minHeight: yellowLayerMinHeight, maxHeight: yellowLayerMaxHeight);
        listTextures[4] = new LayerData("Red - Top", 9, minHeight: redLayerMinHeight, maxHeight: redLayerMaxHeight);
        //listTextures[5] = new LayerData("Red - Top", 0, isDefault: true, minHeight: 0f, maxHeight: 1f);

        return listTextures;
    }

    /*
     * This applies the slope color map to the terrain.
     * The following textures are applied from flat ground (0 degrees) to the steepest point on the terrain (89.8422 degrees):
     * The colors in this configuation highlight subtle changes in slope from 0 degrees to 15 degrees.
     * 
     * Orange (0 - 5 degrees)
     * Yellow (5 - 10 degrees)
     * Teal (10 - 15 degrees)
     * Blue (15 - 89.8422 degrees)
     */
    public static LayerData[] GetSlopeLayerData(TerrainData terrainData)
    {
        // the terrain layers to use.
        LayerData[] listTextures = new LayerData[4];

        // the minimum and maximum slope of the terrain. The maximum slope supported by unity is 90.0f.
        float minTerrainSlope = 0.0f;
        float maxTerrainSlope = 89.8422f;

        // these define the bounds of where each texture appears

        // first layer (orange)
        float orangeLayerMinHeight = 0.0f;
        float orangeLayerMaxHeight = 5.0f;

        // second layer (yellow)
        float yellowLayerMinHeight = orangeLayerMaxHeight;
        float yellowLayerMaxHeight = 10.0f;

        // third layer (teal)
        float tealLayerMinHeight = yellowLayerMaxHeight;
        float tealLayerMaxHeight = 15.0f;

        // fourth layer (blue)
        float blueLayerMinHeight = tealLayerMaxHeight;
        float blueLayerMaxHeight = maxTerrainSlope;

        // fill the listTextures array with slope color data
        listTextures[0] = new LayerData("Orange - 0 - 5 degrees", 1, minSlope: orangeLayerMinHeight, maxSlope: orangeLayerMaxHeight);
        listTextures[1] = new LayerData("Yellow - 5 - 10 degrees", 2, minSlope: yellowLayerMinHeight, maxSlope: yellowLayerMaxHeight);
        listTextures[2] = new LayerData("Teal - 10 - 15 degrees", 3, minSlope: tealLayerMinHeight, maxSlope: tealLayerMaxHeight);
        listTextures[3] = new LayerData("Blue - 15 - 89.8422 degrees", 4, minSlope: blueLayerMinHeight, maxSlope: blueLayerMaxHeight);
        
        return listTextures;
    }
}