using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public List<Light> myLights;
    public DayNightManager dayNight;
    public float minutesLeftToActivate = 30f;

    private MeshRenderer myRenderer;
    private bool lightsOn = false;

    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (dayNight.secondsLeft <= minutesLeftToActivate)
        {
            lightsOn = true;
            TurnOn();
        }
        else lightsOn = false;

        if (!lightsOn)
            TurnOff();
    }

    void TurnOn()
    {
        foreach (var spot in myLights)
        {
            spot.intensity = Mathf.MoveTowards(spot.intensity, 1, Time.deltaTime);
        }     
    }

    void TurnOff()
    {
        foreach (var spot in myLights)
        {
            spot.intensity = Mathf.MoveTowards(spot.intensity, 0, Time.deltaTime);
        }
    }
}
