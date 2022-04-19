using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSeq : MonoBehaviour
{
    
    public Transform center;
    public Transform player;
    public float parallaxRatio;

    void FixedUpdate()
    {
        transform.localPosition = Vector2.Lerp(center.localPosition, player.localPosition, parallaxRatio / 100);
    }
}
