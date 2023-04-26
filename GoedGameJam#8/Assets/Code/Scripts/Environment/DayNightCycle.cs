using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient colour;
    [SerializeField] private Light2D sunLight;
    [SerializeField] private float timeInDay = 250;

    private float time = 1;
    
    void Update()
    {
        if (time > timeInDay)
            time = 0;

        time += Time.deltaTime;
        sunLight.color = colour.Evaluate(time * (1 / timeInDay));
    }
}
