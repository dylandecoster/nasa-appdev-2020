using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;
using static CoordinateConverter;
public class Azmuth
{
    //  (x	33,607,363.58 , y 420966.976, z 0) technical quarnates of the earth
    Transform astronaut;

    public Azmuth(Transform a)
    {
        astronaut = a;

    }

    // Calculates the azimuth and elevation angle of a checkpoint
    public string checkpointString()
    {
        // Creates converters
        CoordinateConverter astroCoordinates = new CoordinateConverter();
        CoordinateConverter earthCoordinates = new CoordinateConverter();

        // Astronaut Coordiantes
        astroCoordinates.setX(astronaut.position.x);
        astroCoordinates.setY(astronaut.position.y);
        astroCoordinates.setZ(astronaut.position.z);

        // Earth Coordinates
        earthCoordinates.setX(33607363.58);
        earthCoordinates.setY(420966.976);
        earthCoordinates.setZ(0);

        // Varibles for azimuth angle
        var latA = astroCoordinates.lat;
        var lonA = astroCoordinates.lon;
        var latB = earthCoordinates.lat;
        var lonB = earthCoordinates.lon;

        // elevation varibles 
        var Xa = astroCoordinates.x;
        var Ya = astroCoordinates.y;
        var Za = astroCoordinates.z;
        var Xb = earthCoordinates.x;
        var Yb = earthCoordinates.y;
        var Zb = earthCoordinates.z;

        //  Azimuth formula
        var azimuth = Atan2((Sin(lonB - lonA) * Cos(latB)), ((Cos(latA) * Sin(latB)) - (Sin(latA) * Cos(latB) * Cos(lonB - lonA))));

        // Finds elevation difference 
        var Xab = Xa - Xb;
        var Yab = Ya - Yb;
        var Zab = Za - Zb;

        var rangeAB = Sqrt((Xab * Xab) + (Yab * Yab) + (Zab * Zab));

        var rZ = Xab * Cos(latA) * Cos(lonA) + Yab * Cos(latA) * Sin(lonA) + Zab * Sin(latA);

        var eAB = Asin(rZ / rangeAB);

        return "Azimuth: " + azimuth.ToString("N3") + "\nElevation: " + eAB.ToString("N3");
    }
}
