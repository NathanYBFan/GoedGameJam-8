using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class LoopManager : MonoBehaviour
{
    public static LoopManager _loopManager { get; private set; }
    [SerializeField] private float counterReset = 1f;

    //defines the "Pattern" of the event
    public delegate void SingleTick();
    // the event itself
    public static event SingleTick tickUpdateEvent;

    private void Awake() { 
        if (_loopManager != null && _loopManager != this) 
            Destroy(this); 
        else 
            _loopManager = this;
        StartCoroutine(TickUpdate());
    }

    public IEnumerator TickUpdate() {
        while (true) {
            yield return new WaitForSeconds(counterReset);
            Debug.Log("Tick");
            tickUpdateEvent();
        }
    }
}
