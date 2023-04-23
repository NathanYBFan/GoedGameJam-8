using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ConveyorAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] conveyorAnimations;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, ReadOnly] private int animationNumber; 

    private void OnEnable() { LoopManager.tickUpdateEvent += StepThroughAnimation; }
    private void OnDisable() { LoopManager.tickUpdateEvent -= StepThroughAnimation; }

    void StepThroughAnimation() {
        animationNumber++;
        if (animationNumber >= conveyorAnimations.Length)
            animationNumber = 0;
        spriteRenderer.sprite = conveyorAnimations[animationNumber];
    }
}
