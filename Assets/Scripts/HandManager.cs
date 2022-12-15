using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static Hand[] hands; // left goes first
    public const int HandPointsNumber = 21;

    [Header("Z must be the same")]
    [SerializeField] private Transform leftBottomPoint;
    [SerializeField] private Transform rightTopPoint;
    [SerializeField] private float depthScaleToPositionCoef;
    [SerializeField] private float shootingForce = 5;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrowPrefab;

    [Space]
    [SerializeField] private Hand handPrefab;

    private Rigidbody arrow;
    private RigRotationSetter[] _rigRotationSetters;
    private RigWrist[] _rigWrists;
    private void Start()
    {
        hands = new Hand[2];
        // _rigRotationSetters = (RigRotationSetter[]) GameObject.FindObjectsOfType(typeof(RigRotationSetter));
        // _rigWrists = (RigWrist[]) GameObject.FindObjectsOfType(typeof(RigWrist));
    }

    public void UpdateHands(string data)
    {
        // creating hands
        if (hands[0] is null || hands[1] is null)
        {
            hands[0] = Instantiate(handPrefab, transform).GetComponent<Hand>();
            hands[1] = Instantiate(handPrefab, transform).GetComponent<Hand>();
            hands[0].Initialize(Label.Left);
            hands[1].Initialize(Label.Right);
        }

        // preparing data
        data = data.Replace("[", "");
        data = data.Replace("]", "");
        data = data.Replace(",", "");
        string[] rawData = data.Split(' ');

        if (rawData.Length == HandPointsNumber * 3 * 2) // validating data
        {
            //throw new ArgumentException("data error");

            // converting data
            Vector3[] leftHandLandmarks = new Vector3[HandPointsNumber];
            Vector3[] rightHandLandmarks = new Vector3[HandPointsNumber];
            float multiplicationCoefficient = leftBottomPoint.position.x - rightTopPoint.position.x; // multiplication
            for (int i = 0; i < HandPointsNumber; i++)
            {
                leftHandLandmarks[i] = new Vector3((float) -Convert.ToDouble(rawData[i * 3]),
                        (float) -Convert.ToDouble(rawData[i * 3 + 1]), (float) Convert.ToDouble(rawData[i * 3 + 2])) *
                    multiplicationCoefficient + leftBottomPoint.position;
            }

            for (int i = HandPointsNumber; i < (HandPointsNumber) * 2; i++)
            {
                rightHandLandmarks[i - HandPointsNumber] = new Vector3((float) -Convert.ToDouble(rawData[i * 3]),
                        (float) -Convert.ToDouble(rawData[i * 3 + 1]), (float) Convert.ToDouble(rawData[i * 3 + 2])) *
                    multiplicationCoefficient + leftBottomPoint.position;
            }

            // scale and relocation
            float leftHandScale = Vector3.Distance(leftHandLandmarks[0], leftHandLandmarks[1]);
            float rightHandScale = Vector3.Distance(rightHandLandmarks[0], rightHandLandmarks[1]);
            float avgScale = (leftHandScale + rightHandScale) / 2;

            float leftScaleCoef = rightHandScale / avgScale;
            float rightScaleCoef = leftHandScale / avgScale;

            for (int i = 0; i < leftHandLandmarks.Length; i++)
            {
                leftHandLandmarks[i].z -= leftScaleCoef * depthScaleToPositionCoef;
                leftHandLandmarks[i] = leftHandLandmarks[0] + (leftHandLandmarks[i] - leftHandLandmarks[0]) * leftScaleCoef;
                rightHandLandmarks[i].z -= rightScaleCoef * depthScaleToPositionCoef;
                rightHandLandmarks[i] = rightHandLandmarks[0] + (rightHandLandmarks[i] - rightHandLandmarks[0]) * rightScaleCoef;
            }
            

            // applying data
            hands[0].UpdatePoints(leftHandLandmarks);
            hands[1].UpdatePoints(rightHandLandmarks);


            if (hands[0].gestureDefined)
            {
                bow.SetActive(true);
                bow.transform.position = (hands[0].handPoints[5].transform.position +
                                         (hands[0].handPoints[8].transform.position +
                                          hands[0].handPoints[4].transform.position) / 2) / 2;
                bow.transform.rotation = Quaternion.LookRotation(hands[0].handPoints[0].transform.position -
                                                                 hands[1].handPoints[0].transform.position);
                // shooting
                if (hands[1].gestureDefined)
                {
                    if (arrow is null)
                    {
                        arrow = Instantiate(arrowPrefab).GetComponent<Rigidbody>();
                        arrow.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    arrow.transform.position = (hands[1].handPoints[4].transform.position +
                                                 hands[1].handPoints[8].transform.position)/2;
                    arrow.transform.rotation = Quaternion.LookRotation(arrow.transform.position - bow.transform.position);
                }
                else
                {
                    if (arrow != null)
                    {
                        arrow.constraints = RigidbodyConstraints.None;
                        arrow.AddForce(
                                (bow.transform.position - arrow.transform.position) *
                                Vector3.Distance(bow.transform.position, arrow.transform.position) * shootingForce, ForceMode.Impulse);
                        arrow = null;
                    }
                }
                
                // bow.transform.rotation = Quaternion.FromToRotation(Vector3.zero, 
                //     hands[1].handPoints[0].transform.position - hands[0].handPoints[0].transform.position);
            }
            else
            {
                bow.SetActive(false);
                if (arrow != null) Destroy(arrow.gameObject);
            }


            // foreach (RigRotationSetter rigRotationSetter in _rigRotationSetters)
            //     rigRotationSetter.UpdateRotation();
            // foreach (RigWrist rigWrist in _rigWrists)
            //     rigWrist.UpdateTransform();
        }
    }
}
