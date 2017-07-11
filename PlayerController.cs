using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Created my Connor Askew
/// Contribution by: Ashley Cheema
/// 
/// this is what controls the player
/// the player is given an area in which he is allowed to move in
/// the player will tilt if they move up or down
/// the player will use fuel as time goes by and keep track of the amount of score the player has gained
/// </summary>

[System.Serializable]
// a small class to define the playable area for the player
// this is used to stop the player from moving if he reaches the edges of the boundary
public class Boundary
{
    /// far left
    /// far right
    /// low down 
    /// high up
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    // the speed at which the player moves
    public float speed;
    public float touchSpeed;

    // the amount we rotate the player by when moving in a certain direction
    // this changes based on the speed above 
    public float tilt;

    // a link to the four floats made above
    // we can set these in the inspector, is put in a section we can minimise so we dont need to always see it and keep the inspector tidy
    public Boundary boundary;

    public GameController GameCont;
    private PlayerController playerCont;
    public GameObject target;
    public GameObject Shield;
    public bool IsShieldActive;
    public float timeLeft;
    private float baseTime;
    public GameObject propeller;

    private float smoothInputHorz;
    private float sensitivity = 4;

    public float smoothSpeed; 
    //public Quaternion touchTilt;    

    void Start()
    {
        baseTime = timeLeft;
    }

    //we are using a rigid body so fixed update would be best
    void FixedUpdate()
    {
        // this gets inputs from the pre-allocated keys defined as Horizontal and Vertical
        //W,A,S,D and the arrow keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // assign a vector3 movement to be equal to a new vector3 which we assign the X and Y to be equal to what the player inputs
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        // times the movement vector3 with a speed to make the object move
        GetComponent<Rigidbody>().velocity = movement * speed;

        //Full movement to move everywhere      
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    // Get movement of the finger since last frame
        //    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //
        //    // Move object across Y plane
        //    transform.Translate(0, touchDeltaPosition.y * touchSpeed, 0);
        //}

        //Static movement, can only go up and down
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            smoothInputHorz = Mathf.Lerp(smoothInputHorz, 1, Time.deltaTime * sensitivity);

            transform.Translate(Vector3.up * Time.deltaTime * speed * smoothInputHorz);

            Quaternion eulerRotate = Quaternion.Euler(0.0f, 180.0f, -10.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, eulerRotate, smoothSpeed * Time.deltaTime);

        }
        //else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    // Get movement of the finger since last frame
        //    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //
        //    transform.Translate(touchDeltaPosition.x * -touchSpeed, 0, 0);
        //    // Move object across XY plane
        //    //transform.Translate(touchDeltaPosition.x * touchSpeed, touchDeltaPosition.y * touchSpeed, 0);
        //}
        else
        {
            smoothInputHorz = Mathf.Lerp(smoothInputHorz, 1, Time.deltaTime * sensitivity);
            transform.Translate(-Vector3.up * Time.deltaTime * speed * smoothInputHorz);

            Quaternion eulerRotateReturn = Quaternion.Euler(0.0f, 180.0f, 4.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, eulerRotateReturn, smoothSpeed * Time.deltaTime);
        }

        // here is where we use the class boundary's floats to "clamp" or confine the player inside an invisible square, 
        // it's a square because we dont confine the player on the Z axis, we want a simple up, down, left and right moving player, simple
        // similar to random.range to get a number inbetween two points, heere we assign an axis to clamp, then a minimum and a maximum value
        // these need to be assigned before running it, moving the player in the scene to find a suitable set of numbers
        // can easily be changed in the inspector and the player will have a new area to play in
        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody>().position.y, boundary.yMin, boundary.yMax),
            0.0f
        );

        //GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 180.0f, GetComponent<Rigidbody>().velocity.y * -tilt);

    }

    void Update()
    {

        if (IsShieldActive == true)
        {
            Shieldtimer();
        }
        //Makes the player's propeller spin
        propeller.transform.Rotate(0, 0, Time.deltaTime * 250);

    }

    //A simple trigger to increase score
    //This also calls from another script which controls a lot of the game
    void OnTriggerEnter(Collider other)
    {
        //If the collectable has a tag of "Collectable"
        //Then find ScoreChanger in the GameController
        if (other.gameObject.tag == "Collectable")
        {
            GameCont.ScoreChanger();
            other.gameObject.GetComponent<Renderer>().material.color = Color.green;

            ParticleSystem pS = GameObject.Find("CropWater").GetComponent<ParticleSystem>();
            pS.Play();
        }

        //The same is done with the fuel
        //But instead this gets destoryed
        //This is a good indicator that it's been collected
        if (other.gameObject.tag == "Fuel")
        {
            Destroy(other.gameObject);
            GameCont.FuelChanger();
        }

        if (other.gameObject.tag == "ShieldPickUp")
        {
            Shield.SetActive(true);
            IsShieldActive = true;
            Destroy(other.gameObject);
        }
        //If the player hits either of these it will end the game
        if (other.gameObject.tag == "Hay" || other.gameObject.tag == "Ground" || other.gameObject.tag == "Bird")
        {
            GameCont.ShowEndGameUI();
        }
    }
    //A timer for when the shield is turned off
    void Shieldtimer()
    {
        timeLeft -= Time.deltaTime;

        if ( timeLeft < 0 )
        {
            Shield.SetActive(false);
            IsShieldActive = false;
            timeLeft = baseTime;
        }
    }

    

}
