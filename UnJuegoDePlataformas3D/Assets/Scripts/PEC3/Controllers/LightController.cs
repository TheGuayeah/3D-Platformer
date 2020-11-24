using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Material normalMaterial;
    public Material lightedMaterial;
    public GameObject myLights;
    public DayNightManager dayNight;
    public float minutesLeftToActivate = 30f;

    private MeshRenderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        TurnOff();
    }

    void Update()
    {
        if (dayNight.secondsLeft <= minutesLeftToActivate)
            TurnOn();
    }

    void TurnOn()
    {
        myLights.SetActive(true);
        myRenderer.material = lightedMaterial;
    }

    void TurnOff()
    {
        myLights.SetActive(false);
        myRenderer.material = normalMaterial;
    }
}
