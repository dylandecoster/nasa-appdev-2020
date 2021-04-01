// Turns latitude and longitude into x y
using System;
public class CoordinateConverter
{
    // All the data about the point
    public double lat;
    public double lon;
    public double height;
    public double x;
    public double z;
    public double y;

    // defualt constructor
    public CoordinateConverter(): this(-90.0, 0, 100.0){}

    public CoordinateConverter(double lat, double lon) : this(lat, lon, 100.0) { }

    // Constructor
    public CoordinateConverter (double lat, double lon, double height)
    {
        this.lat = lat;
        this.lon = lon;
        this.height = height;
        setXYZ();
    }

    // Does the trig math to turn angles into x and z
    public virtual void setXYZ()
    {
        double radius = (173400 + height) / 2.995;
       

        x = radius * Math.Cos((Math.PI / 180) * lat) * Math.Cos((Math.PI / 180) * -lon);
        z = radius * Math.Cos((Math.PI / 180) * lat) * Math.Sin((Math.PI / 180) * -lon);
        y = -1 * (2 + height) * Math.Sin((Math.PI / 180) * lat);
    }


    // Does the trigmath to turn x and z into an angle and radius 
    private void setLatLonHeight()
    {
        lon = (180 / Math.PI) * Math.Atan(z/ x);
        lat = (180 / Math.PI) * Math.Atan(Math.Sqrt((x * x) + (z * z) / y));
        height = -1 * (180 / Math.PI) * Math.Sqrt((x * x) + (y * y) + (z * z));
    }


    public void setLat(double lat)
    {
        this.lat = lat;
        setXYZ();
    }

    public void setLon(double lon)
    {
        this.lon = lon;
        setXYZ();
    }

    public void setHeight(double height)
    {
        this.height = height;
        setXYZ();
    }

    public void setX(double x)
    {
        this.x = x;
        setLatLonHeight();
    }

    public void setY(double y)
    {
        this.y = y;
        setLatLonHeight();
    }

    public void setZ(double z)
    {
        this.z = z;
        setLatLonHeight();
    }

}