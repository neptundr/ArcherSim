using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject handPoint;
    
    public Transform[] handPoints;
    
    private void Start()
    {
        // hand points creation
        handPoints = new Transform[Convert.ToInt32(HandManager.HandPointsNumber)];
        for (int i = 0; i < handPoints.Length; i++)
        {
            handPoints[i] = Instantiate(handPoint, transform).transform;
        }
    }
    
    public void UpdatePoints(Vector3[] newPositions)
    {
        // validating
        if (newPositions.Length != handPoints.Length) throw new AggregateException();

        for (int i = 0; i < handPoints.Length; i++)
        {
            handPoints[i].position = newPositions[i];
        }
    }
}
