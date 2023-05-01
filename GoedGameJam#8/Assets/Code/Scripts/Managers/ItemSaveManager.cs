using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class ItemSaveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bearCounter;
    [SerializeField] private TextMeshProUGUI cowCounter;
    [SerializeField] private TextMeshProUGUI foxCounter;
    [SerializeField] private TextMeshProUGUI sheepCounter;
    [SerializeField] private TextMeshProUGUI rabbitCounter;
    [SerializeField] private TextMeshProUGUI fishCounter;

    [SerializeField, ReadOnly] private int numberOfBears, numberOfCows, numberOfFish, numberOfFoxes, numberOfRabbits, numberOfSheep;
   
 
    private void Update()
    {

        SetNumberOfBears();
        SetNumberOfCows();
        SetNumberOfFoxes();
        SetNumberOfSheep();
        SetNumberOfFish();
        SetNumberOfRabbits();

    }

    // Bears
    public void SetNumberOfBears()
    {

        numberOfBears = GameObject.FindGameObjectsWithTag("Bears").Length;
        bearCounter.text = numberOfBears.ToString();
    }
    // Cows
    public void SetNumberOfCows()
    {
        numberOfCows = GameObject.FindGameObjectsWithTag("Cows").Length;
        cowCounter.text = numberOfCows.ToString();
    }
    // Foxes
    public void SetNumberOfFoxes()
    {
        numberOfFoxes = GameObject.FindGameObjectsWithTag("Foxes").Length;
        foxCounter.text = numberOfFoxes.ToString();
    }
    // Sheep
    public void SetNumberOfSheep()
    {
        numberOfSheep = GameObject.FindGameObjectsWithTag("Sheep").Length;
        sheepCounter.text = numberOfSheep.ToString();
    }
    // Rabbit
    public void SetNumberOfRabbits()
    {
        numberOfRabbits = GameObject.FindGameObjectsWithTag("Rabbits").Length;
        rabbitCounter.text = numberOfRabbits.ToString();
    }
    // Fish
    public void SetNumberOfFish()
    {
        numberOfFish = GameObject.FindGameObjectsWithTag("Fish").Length; 
        fishCounter.text = numberOfFish.ToString();
    }


}
