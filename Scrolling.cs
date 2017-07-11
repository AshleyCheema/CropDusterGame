using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour
{

    public float speed;
    public float tileSizeZ;
    private Vector3 startPosition;


	void Start ()
    {
        startPosition = transform.position;
    }

    //To create a scrolling effect, Mathf.Repeat was used
    //This uses time which gives it the scrolling effect and will also continiously loop
    void Update ()
    {
        float newPosition = Mathf.Repeat(Time.time * speed, tileSizeZ);
        transform.position = startPosition + Vector3.left * newPosition;

	}
}
