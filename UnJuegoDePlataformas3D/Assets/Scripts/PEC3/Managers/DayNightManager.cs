using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [Header("Lightning")]
    public GameObject sunLight;
    public float totalSecondsTo16hours = 90f;
    public float startDayRotationX = 0f;
    public float endDayRotationX = 270f;
    public float secondsLeft;

    private float elapsedTime = 0f;

    private void Awake()
    {
        if (sunLight == null)
            sunLight = gameObject;
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        secondsLeft = totalSecondsTo16hours - elapsedTime;

        float rot = (endDayRotationX - startDayRotationX) / totalSecondsTo16hours;
        float currentX = rot * Time.deltaTime;
        
        sunLight.transform.Rotate(currentX, 0, 0);

        // Debug
        if (Input.GetKeyDown(KeyCode.R)) 
            Reset();
    }

    public void Reset()
    {
        totalSecondsTo16hours--;
        elapsedTime = 0;
        sunLight.transform.rotation = Quaternion.Euler(startDayRotationX, -90, 0);
    }
}
