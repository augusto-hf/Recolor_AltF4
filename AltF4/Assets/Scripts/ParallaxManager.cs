using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform ImageToParallax;
    Vector2 startPosition;
    float startZ;

    Vector2 travel =>  (Vector2)cam.transform.position - startPosition;

    float distanceFromSubject => transform.position.z - ImageToParallax.position.z;

    float clippingPlane => cam.transform.position.z + (distanceFromSubject > 0? cam.farClipPlane : cam.nearClipPlane);

    float parallaxFactor => Mathf.Abs(distanceFromSubject/clippingPlane);
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    
    void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
