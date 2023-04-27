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

    private void Awake() {
        ResetAllCounters();
    }
    private void ResetAllCounters() {
        SetNumberOfBears(0);
        SetNumberOfCows(0);
        SetNumberOfFish(0);
        SetNumberOfFoxes(0);
        SetNumberOfRabbits(0);
        SetNumberOfSheep(0);
    }

    // Bears
    public int GetNumberOfBears() { return numberOfBears; }
    public void SetNumberOfBears(int newCount) {
        numberOfBears = newCount;
        bearCounter.text = numberOfBears.ToString();
    }
    // Cows
    public int GetNumberOfCows() { return numberOfCows; }
    public void SetNumberOfCows(int newCount) {
        numberOfCows = newCount;
        cowCounter.text = numberOfCows.ToString();
    }
    // Foxes
    public int GetNumberOfFoxes() { return numberOfFoxes; }
    public void SetNumberOfFoxes(int newCount) {
        numberOfFoxes = newCount;
        foxCounter.text = numberOfFoxes.ToString();
    }
    // Sheep
    public int GetNumberOfSheep() { return numberOfSheep; }
    public void SetNumberOfSheep(int newCount) {
        numberOfSheep = newCount;
        sheepCounter.text = numberOfSheep.ToString();
    }
    // Rabbit
    public int GetNumberOfRabbits() { return numberOfRabbits; }
    public void SetNumberOfRabbits(int newCount) {
        numberOfRabbits = newCount;
        rabbitCounter.text = numberOfRabbits.ToString();
    }
    // Fish
    public int GetNumberOfFish() { return numberOfFish; }
    public void SetNumberOfFish(int newCount) {
        numberOfFish = newCount;
        fishCounter.text = numberOfFish.ToString();
    }
}
