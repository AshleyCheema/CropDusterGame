using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour
{
    public float RotationAmount;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, RotationAmount, 0);
	}
}
