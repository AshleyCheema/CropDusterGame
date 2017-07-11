using UnityEngine;
using System.Collections;

public class HouseMover : MonoBehaviour
{
    public float Speed;

	// Use this for initialization
	void Start ()
    {
        //Speed = 20;
    }

    // Update is called once per frame
    void Update ()
    { 
           transform.Translate(new Vector3(-Speed, 0, 0) * Time.deltaTime);
    }
}
