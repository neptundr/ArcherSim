using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public const int HandPointsNumber = 21 * 3;
    
    [Header("Z must be the same")]
    [SerializeField] private Transform leftBottomPoint;
    [SerializeField] private Transform rightTopPoint;
    // [SerializeField] private float scaleCoefficient = 15;
    [Space]
    [SerializeField] private Hand handPrefab;

    private Hand leftHand;
    private Hand rightHand;

    public void UpdateHands(string data)
    {
        // creating hands
        if (leftHand is null || rightHand is null)
        {
            leftHand = Instantiate(handPrefab, transform).GetComponent<Hand>();
            rightHand = Instantiate(handPrefab, transform).GetComponent<Hand>();
        }
        
        // preparing data
        data = data.Replace("[", "");
        data = data.Replace("]", "");
        data = data.Replace(",", "");
        string[] rawData = data.Split(' ');

        // validating data
        if (rawData.Length != HandPointsNumber) throw new ArgumentException("data error");

        // converting data
        Vector3[] handLandmarks = new Vector3[HandPointsNumber];
        float multiplicationCoefficient = leftBottomPoint.position.x - rightTopPoint.position.x; // multiplication
        for (int i = 0; i < HandPointsNumber/3; i++)
        {
            handLandmarks[i] = new Vector3((float) -Convert.ToDouble(rawData[i * 3]),
                     (float) -Convert.ToDouble(rawData[i * 3 + 1]), (float) Convert.ToDouble(rawData[i * 3 + 2])) *
                multiplicationCoefficient + leftBottomPoint.position;
        }
        
        // applying data
        rightHand.UpdatePoints(handLandmarks);
    }
}
