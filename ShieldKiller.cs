using UnityEngine;
using System.Collections;

public class ShieldKiller : MonoBehaviour
{

    private GameObject Hayrelocator;


	// Use this for initialization
	void Start ()
    {
        Hayrelocator = GameObject.FindGameObjectWithTag("Hayrelocator");
        Debug.Log(Hayrelocator);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //Deletes the object when it comes into contact with the shield
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            Debug.Log("Hit Bird");
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Hay")
        {
            Debug.Log("Hit Hay");
            other.transform.position = Hayrelocator.transform.position;
        }


    }
}
