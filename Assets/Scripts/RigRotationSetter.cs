using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigRotationSetter : MonoBehaviour
{
    [SerializeField] private int previousPoint;
    [SerializeField] private int anglePoint;
    [SerializeField] private int nextPoint;
    [SerializeField] private int handIndex;

    public void UpdateRotation()
    {
        // coping rotation of landmarks
        transform.localRotation = Quaternion.FromToRotation(
            HandManager.hands[handIndex].handPoints[nextPoint].position -
            HandManager.hands[handIndex].handPoints[anglePoint].position,
            HandManager.hands[handIndex].handPoints[anglePoint].position -
            HandManager.hands[handIndex].handPoints[previousPoint].position);
    }
}
