using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y; 
    }
    void FixedUpdate()
    {
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float distance = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector2(transform.position.x, startpos + distance);

        if(temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
