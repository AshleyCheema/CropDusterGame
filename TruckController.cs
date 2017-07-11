using UnityEngine;
using System.Collections;

public class TruckController : MonoBehaviour
{

    public float Speed;
    public GameObject restartPos;

	void Start ()
    {
        restartPos = GameObject.FindWithTag("Respawn");
        Speed = 5;
	}
	
	void Update ()
    {
            transform.Translate(new Vector3(0, 0, Speed) * Time.deltaTime);
    }

    public void ResetTruck()
    {
        transform.position = restartPos.transform.position;
    }

   //private void OnTriggerEnter(Collider other)
   //{
   //    if(other.gameObject.tag == "TruckStopper")
   //    {
   //        timeToStop = true;
   //    }
   //}
}
