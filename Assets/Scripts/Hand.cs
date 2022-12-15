using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject handPoint;
    
    public Transform[] handPoints;
    public float deltaXMove = 0.1f;
    public bool gestureDefined = false;

    private Vector3[] _handPointsToPosition;
    private Label _label;

    private bool _started = false;

    private void Start()
    {
        // hand points creation
        handPoints = new Transform[Convert.ToInt32(HandManager.HandPointsNumber)];
        for (int i = 0; i < handPoints.Length; i++)
        {
            handPoints[i] = Instantiate(handPoint, transform).transform;
        }
    }

    public void Initialize(Label label)
    {
        _label = label;
    }

    private void Update()
    {
        if (_started)
        {
            // smooth moving
            for (int i = 0; i < handPoints.Length; i++)
            {
                handPoints[i].position = Vector3.MoveTowards(handPoints[i].position, _handPointsToPosition[i], deltaXMove);
            }
        
            // defining gestures
            if (_label == Label.Left)
            {
                if (Mathf.Abs((Vector3.Distance(handPoints[8].position, handPoints[12].position) +
                     Vector3.Distance(handPoints[12].position, handPoints[16].position) +
                     Vector3.Distance(handPoints[16].position, handPoints[20].position)) / 3 
                    - Vector3.Distance(handPoints[4].position, handPoints[12].position)) < 0.5)
                {
                    gestureDefined = true;
                    Debug.Log("Bow setup");
                }
                else
                {
                    gestureDefined = false;
                }
            }
            
            if (_label == Label.Right)
            {
                if (Vector3.Distance(handPoints[4].position, handPoints[8].position) <
                    Vector3.Distance(handPoints[5].position, handPoints[7].position))
                {
                    gestureDefined = true;
                    Debug.Log("Arrow setup");
                }
                else
                {
                    gestureDefined = false;
                }
            }
        }
    }

    public void UpdatePoints(Vector3[] newPositions)
    {
        // validating
        if (newPositions.Length != handPoints.Length) throw new AggregateException();
        _started = true;
        _handPointsToPosition = newPositions;
    }
}

public enum Label
{
    Right,
    Left
}