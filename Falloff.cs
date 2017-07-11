using UnityEngine;
using System.Collections;
/// <summary>
/// Created by: Ashley Cheema
/// 
/// 
/// </summary>
public class Falloff : MonoBehaviour
{

    public GameObject truck;

    private Vector3 offset;

    private bool timeToActuallyJump;
    private bool timeToJump;
    private bool timeToStopJumping;
    private bool timeToStopAddingForce;
    private bool leftMove;


    public Rigidbody rb;
    private int x, y;
    public float speed;
    public int yMin, yMax;
    public int xMin, xMax;

    private float jumptimer;
    public float jumptimerLower;
    public float jumptimerUpper;

    public GameObject RespawnPos;
    //public Quaternion ResetRotation;

    void Start()
    {
        //ResetRotation = Quaternion.Euler(new Vector3(0, 30, 0));
        rb = GetComponent<Rigidbody>();

        timeToActuallyJump = false;
        timeToStopJumping = true;
        timeToJump = false;
        timeToStopAddingForce = false;
        leftMove = false;

        offset = transform.position - truck.transform.position;

        RespawnPos = GameObject.FindGameObjectWithTag("Hayrelocator");
    }

    void LateUpdate()
    {
        if (timeToJump == false)
        {
            transform.position = truck.transform.position + offset;
        }
        if(timeToActuallyJump == true && timeToStopJumping == true)
        {
            StartCoroutine(JumpDelay());
        }

        if(leftMove == true)
        {
            transform.Translate(new Vector3(0, 0, -2) * Time.deltaTime);
        }
    }

    //The IEnumerator gives it a random time when to fly off
    IEnumerator JumpDelay()
    {        
        jumptimer = Random.Range(jumptimerLower, jumptimerUpper);
        yield return new WaitForSeconds(jumptimer);
        timeToJump = true;
        TimeToFly();
    }

    //This function makes the haybales shoot off the truck in an unpredictable way 
    void TimeToFly()
    {
        if (timeToStopAddingForce == false)
        {
            y = Random.Range(yMin, yMax);
            x = Random.Range(xMin, xMax);

            Vector3 movement = new Vector3(x, y, 0.0f);
            rb.AddForce(movement * speed);

            timeToStopJumping = false;
            timeToStopAddingForce = true;
        }
    }
    //This triggers the haybales jumping off
    //Then when they hit the ground they move to the left off screen and wait to respawn
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "JumpTrigger")
        {
            timeToActuallyJump = true;
        }

        if(other.gameObject.tag == "Ground")
        {
            leftMove = true;
        }
    }
    //This resets the haybales back into their first position
    public void HayReseter()
    {
        /*
        transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        Debug.Log(transform.rotation);
        */
        //rb.velocity = Vector3.zero;
        transform.position = RespawnPos.transform.position;
        //rb.velocity = Vector3.zero;

        timeToActuallyJump = false;
        timeToStopJumping = true;
        timeToJump = false;
        timeToStopAddingForce = false;
        leftMove = false;

        rb.velocity = Vector3.zero;

    }





}
