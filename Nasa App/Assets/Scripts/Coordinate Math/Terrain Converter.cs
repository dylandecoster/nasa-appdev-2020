using System;
using static CoordinateConverter;

// Somehow I completely forgot how do due subclasses so I just remade the entire thing with one thing different 
public class TerrainConverter 
{
    // All the data about the point
    public double lat;
    public double lon;
    public double x;
    public double z;


    // defualt constructor
    public TerrainConverter() { }

    // Constructor
    public TerrainConverter(double lat, double lon)
    {
        this.lat = lat;
        this.lon = lon;
        setXZ();
    }

    // Does the trig math to turn angles into x and z
    public virtual void setXZ()
    {
        double radius = (90 + lat) * 500;
        double angle = -lon;



        x = radius * Math.Cos((Math.PI / 180) * angle);
        z = radius * Math.Sin((Math.PI / 180) * angle);
    }


    // Does the trigmath to turn x and z into an angle and radius 
    private void setLatLon()
    {
        double radius = 0;
        double angle = 0;



        // r = √ ( x2 + y2 )
        radius = Math.Sqrt((x * x) + (z * z));

        // θ = tan-1 ( y / x )
        angle = Math.Atan(z / x);


        lon = -angle;

        lat = radius / 500 - 90;
    }


    public void setLat(double lat)
    {
        this.lat = lat;
        setXZ();
    }

    public void setLon(double lon)
    {
        this.lon = lon;
        setXZ();
    }

    public void setX(double x)
    {
        this.x = x;
        setLatLon();
    }

    public void setZ(double z)
    {
        this.z = z;
        setLatLon();
    }

}
