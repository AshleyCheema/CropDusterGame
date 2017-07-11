using UnityEngine;
using System.Collections;

public class RepeatingGroung : MonoBehaviour
{

    public float Road_Speed;
    private Vector3 end_position;
    private Vector3 Reset_position;

    private void Awake ()
    {
        end_position = new Vector3(-280f, -8.8f,-1.1f);
        Reset_position = new Vector3(278f, -8.8f,-1.1f);
	}
	
	void Update ()
    {
        transform.Translate(-Road_Speed, 0, 0);
        if (transform.position.x < end_position.x)
        {
            RepositionBackground();
        }
	}

    private void RepositionBackground()
    {        
        transform.position = Reset_position;
    }
}
