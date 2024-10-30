using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloattingImage : MonoBehaviour
{
    public float flySpeed = 1f;
    public float flyHeight = 0.5f;
    Vector3 startposition;
    void Start()
    {
        startposition = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * flySpeed) * flyHeight;
        transform.position = new Vector3(startposition.x, startposition.y + newY, startposition.z);
    }
}
