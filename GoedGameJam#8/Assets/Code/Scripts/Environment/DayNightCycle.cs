using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{

    [SerializeField] private Gradient colour;
    [SerializeField] private Light2D sunLight;

    [SerializeField] private float timeInDay = 250;

    private float time = 1;

    private bool canDayBeChanged = true;
    // Update is called once per frame
    void Update()
    {

        if (time > timeInDay)
            time = 0;


        if((int) time == timeInDay/2 && canDayBeChanged)
            canDayBeChanged = false;

        if ((int)time == ((timeInDay/2) + 1))
            canDayBeChanged = true;

        time += Time.deltaTime;
        sunLight.color = colour.Evaluate(time * (1/timeInDay));
    }
}
