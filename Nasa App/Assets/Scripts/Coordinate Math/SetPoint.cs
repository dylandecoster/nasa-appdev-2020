using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CoordinateConverter;

public class SetPoint : MonoBehaviour
{

    public GameObject gameObject;
    public double lat;
    public double lon;
    public double height;
    private CoordinateConverter converter;

    // Start is called before the first frame update
    void Start()
    {
        converter = new CoordinateConverter(lat, lon, height);
        gameObject.transform.position = new Vector3((float)converter.x, (float)converter.y, (float)converter.z);
    }

    // Update is called once per frame
    void Update()
    {
        converter.setLat(lat);
        converter.setLon(lon);
        converter.setHeight(height);
        gameObject.transform.position = new Vector3((float)converter.x, (float)converter.y, (float)converter.z);
    }
}
