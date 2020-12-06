using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightManager : MonoBehaviour
{
    [Header("Lightning")]
    public GameObject sunLight;
    public float totalSecondsTo16hours = 90f;
    public float startDayRotationX = 0f;
    public float endDayRotationX = 270f;
    public float secondsLeft;
    public Text displayedTime;

    private MenuManager menuManager;
    private float elapsedTime = 0f;

    private void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>().GetComponent<MenuManager>();
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

        SetText();

        if (IsDead()) menuManager.GoToScene("GameOver");
    }

    public void Reset()
    {
        totalSecondsTo16hours--;
        elapsedTime = 0;
        sunLight.transform.rotation = Quaternion.Euler(startDayRotationX, -90, 0);
        SetText();
    }

    private void SetText()
    {
        float secondsXHour = totalSecondsTo16hours / 16f;
        int hour = (int)((totalSecondsTo16hours - secondsLeft) / secondsXHour);
        int minutes = (int)((((totalSecondsTo16hours - secondsLeft) / secondsXHour) - hour) * 60);
        string label;

        if ((hour + 8) > 12)
        {
            label = (hour + 8 - 12).ToString("00") + ":" + minutes.ToString("00") + "F G"; // Por la tarde
            //if((hour + 8 - 12) > 12)
            //    Reset();
        }
        else
            label = (hour + 8).ToString("00") + ":" + minutes.ToString("00") + "E G"; // Por la mañana

        displayedTime.text = label;
    }

    private bool IsDead()
    {
        return secondsLeft < 0;
    }
}
