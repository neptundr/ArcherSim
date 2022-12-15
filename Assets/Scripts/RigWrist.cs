using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigWrist : MonoBehaviour
{
    [SerializeField] private int handIndex;

    public void UpdateTransform()
    {
        transform.position = HandManager.hands[handIndex].handPoints[0].position;
        transform.localRotation = Quaternion.FromToRotation(
            HandManager.hands[handIndex].handPoints[9].localPosition -
            HandManager.hands[handIndex].handPoints[0].localPosition,
            Vector3.up);
    }
}
