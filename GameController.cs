using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Created by: Connor Askew
/// Contributed by: Ashley Cheema
/// 
/// This is used to control the game
/// There is a random time between spawning houses 
/// Occasionally it spawns the crop that the plane needs to dust 
/// </summary>
/// 

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/* CHEWY!
 * put a shader on a gameobject Connor will spawn on the player
 * then it will look like a shield that protects the player with either a certain amount of charges or for a duration of time
 * also put the same effect/shader on a collectable so the player can pick it up to collect the shield
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class GameController : MonoBehaviour
{


    //House Spawner Stuff
    [Header("HouseSpawner Stuff")]
    public GameObject[] House; // array of house prefabs
    public GameObject Crop; // the crop prefab
    private GameObject NextHouse; // a place holder which changed based on what prefab we want to spawn
    private float HouseSpeed = -20;
    private float HouseTimeGap; // the time between spawning houses

    public float HouseTimeGapLower; // lower half of the range between spawning houses
    public float HouseTimeGapUpper; // higher half
    private GameObject HouseSpawnPos; // the place we spawn the houses

    // Crop Stuff
    [Header("Crop Stuff")]
    private float CropTimeGap; // similar to the house this is the time between crops appearing
    private bool CropSpawnTimeUp; // the bool we use to control when to spawn the crop
    public float CropTimeGapLower; // in the inspector this will be significantly higher on both upper and lower because we don't want the crops to be too close together
    public float CropTimeGapUpper; // higher half

    //Bird Obstacle
    [Header("Bird Stuff")]
    public GameObject bird;
    private GameObject NextBird;
    public float birdPosHigh, birdPosLow;
    public float birdSpawnTimeHigh, birdSpawnTimeLow;
    private GameObject birdSpawnPos;
    private float birdTimer;
    private float birdSpeed = 20;

    //power up stuff
    [Header("Power Up Stuff")]
    public GameObject PowerUp;
    public float PUSpawnTimeHigh, PUSpawnTimeLow;
    private float PUTimer;


    //Score UI Stuff
    [Header("UI Stuff")]
    private int score = 0;
    public Text scoreText, endScore;
    public bool paused;
    public GameObject startButton;
    public Text leaderScore;

    //Fuel UI Stuff
    [Header("Fuel Stuff")]    
    private float fuel = 100;
    public Text fuelText;
    public float fuelDecreaseRate;
    public GameObject FuelObject;
    private float FuelTimeGap;
    public float FuelTimeGapLower;
    public float FuelTimeGapUpper;
    private bool FuelSpawnTimeUp;

    //Truck Stuff
    [Header("Truck Stuff")]
    public GameObject truck;
    public int lowerTruck, higherTruck;
    public TruckController TruckCont;
    public Falloff[] FallOffCont;

    //Other Stuff
    [Header("Other Stuff")]
    public PlayerController playerCont;
    public GameObject explosion;
    bool hasSpedUp;
    bool StopSpawning;
    private GameObject[] CropClones;

    [Header("Menu Stuff")]
    //Menu Stuff
    public GameObject endGameUI;

    void Start()
    {
        HouseSpawnPos = GameObject.FindGameObjectWithTag("HouseSpawner"); // finding the hosue object without dragging and dropping in the inspector
        birdSpawnPos = GameObject.FindGameObjectWithTag("BirdSpawner");

        StartCoroutine(HouseMaker()); // controls the houses
        StartCoroutine(CropTimer()); // controls the crop

        StartCoroutine(GenerateBird()); // controls the birds
        StartCoroutine(FuelTimer()); // controls the fuel

        StartCoroutine(GenerateTrucks()); // controls the trucks

        StartCoroutine(FuelDecrease()); // controls how much fuel we use over time
        StartCoroutine(GeneratePowerUp()); // controls how much fuel we use over time
        Crop.gameObject.GetComponent<Collider>().enabled = true;

        StopSpawning = false;


        paused = true;
        Time.timeScale = 0;
    }

    IEnumerator CropTimer()
    {
        while (!StopSpawning)
        {
            CropTimeGap = Random.Range(CropTimeGapLower, CropTimeGapUpper); // changes the value each time we enter so it varies each time, gets a random range between the variables
            CropSpawnTimeUp = true;// its time to spawn the crop
            yield return new WaitForSeconds(CropTimeGap); // waits for the time we made earlier
            //CropSpawnTimeUp = true;// its time to spawn the crop
        }
    }

    IEnumerator FuelTimer()
    {
        while (true)
        {
            FuelTimeGap = Random.Range(FuelTimeGapLower, FuelTimeGapUpper); // changes the value each time we enter so it varies each time, gets a random range between the variables
            yield return new WaitForSeconds(FuelTimeGap); // waits for the time we made earlier
            FuelSpawnTimeUp = true;// its time to spawn the fuel
        }
    }

    IEnumerator FuelDecrease()
    {
        while (true)
        {
            fuel -= 1;
            yield return new WaitForSeconds(fuelDecreaseRate); // waits for the time we made earlier
        }
    }

    IEnumerator GenerateBird()
    {
        while (true)
        {
            if (FuelSpawnTimeUp == true) // if its time to spawn the fuel
            {
                NextBird = FuelObject; // sets the next house to spawn to be the fuel
                FuelSpawnTimeUp = false; // change the bool to be false so we dont always spawn the crop, we do it once then it moves on
            }
            else
            {
                NextBird = bird;
            }

            Vector3 randomBirdPos = new Vector3(birdSpawnPos.transform.position.x, Random.Range(birdPosHigh, birdPosLow), birdSpawnPos.transform.position.z);
            Quaternion birdRotation = Quaternion.identity;

            GameObject InstantiateBird = (GameObject)Instantiate(NextBird, randomBirdPos, birdRotation);
            InstantiateBird.GetComponent<HouseMover>().Speed = birdSpeed;
            birdTimer = Random.Range(birdSpawnTimeLow, birdSpawnTimeHigh);
            
            yield return new WaitForSeconds(birdTimer);
        }
    }

    IEnumerator GeneratePowerUp()
    {
        while (true)
        {
            Vector3 randomBirdPos = new Vector3(birdSpawnPos.transform.position.x, Random.Range(birdPosHigh, birdPosLow), birdSpawnPos.transform.position.z);
            Quaternion PURotation = Quaternion.identity;
            Instantiate(PowerUp, randomBirdPos, PURotation);
            PUTimer = Random.Range(PUSpawnTimeHigh, PUSpawnTimeHigh);
            yield return new WaitForSeconds(PUTimer);
        }
    }

    IEnumerator HouseMaker()
    {
        while (true)
        {
            if (CropSpawnTimeUp == true && playerCont.enabled == true) // if its time to spawn the crop
            {
                NextHouse = Crop; // sets the next hosue to spawn to be the crop
                CropSpawnTimeUp = false; // change the bool to be false so we dont always spawn the crop, we do it once then it moves on
            }
            else
            {
                int HouseNumber = Random.Range(0, House.Length); //this gets a Random number between the size of the array
                NextHouse = House[HouseNumber];// sets the next hosue to spawn to be one of the houses in the array
            }
            GameObject InstantiateHouse = (GameObject)Instantiate(NextHouse, HouseSpawnPos.transform.position, HouseSpawnPos.transform.rotation); // create the next building specified previously at the House spawn location
            InstantiateHouse.GetComponent<HouseMover>().Speed = HouseSpeed;
            HouseTimeGap = Random.Range(HouseTimeGapLower, HouseTimeGapUpper); // changes the float each time we enter so it changes the time to wait before we spawn another building
            yield return new WaitForSeconds(HouseTimeGap); // wait until we can spawn another house again and then go back to the beginning and do this all over again
        }
    }

    //Similar to the above IEnumerators except for the truck
    //Except this time it resets the truck back to it's original position
    //And the IF statement is for the haybales so that they return to their original position when it is reset
    IEnumerator GenerateTrucks()
    {
        while (true)
        {            
            //int generateTruck = Random.Range(lowerTruck, higherTruck);
            yield return new WaitForSeconds(18);
            TruckCont.ResetTruck();
            for (int i = 0; i < FallOffCont.Length; ++i)
            {
                FallOffCont[i].HayReseter();
            }
        }
    }

    void Update()
    {
        //This makes it so overtime the fuel should slowly decrease
        //If the fuel is less than or equal too 100, decrease at fuelDecreaseRate set speed
        //This is than outputted to the UI text and rounded so that it only goes down in whole numbers
        if (fuel <= 100)
        {
            fuelText.text = "Fuel:" + Mathf.Round(fuel);
        }

        //And once the fuel hits 0 then play death animation (or something)
        if (fuel <= 0)
        {
            fuel = 0;
            ShowEndGameUI();  
        }

        //This is the modulos operation, so every 50 points this will increment the speed once
        if (score % 50 == 0)
        {
            //Making sure it does not increment when the score is at 0
            if (score != 0)
            {
                //Bool to say that when hasSpedUp is false it will call the function and set it to true so that it only goes through this once
                if (hasSpedUp == false)
                {
                    hasSpedUp = true;
                    SpeedUp();
                } 
            }
        }
        else
        {
            hasSpedUp = false;
        }

    }
    
    //A function to set to do the below when called
    //In this instance it is called when it's game over
    public void ShowEndGameUI()
    {
        endGameUI.SetActive(true);
        playerCont.enabled = false;
        explosion.SetActive(true);

        //PlayerPrefs saves the last highest score that the player got
        if (PlayerPrefs.HasKey("Highscore"))
        {
           if (PlayerPrefs.GetInt("Highscore") < score)
            {
                PlayerPrefs.SetInt("Highscore", score);
            }
            leaderScore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    //Simply adds to the score
    public void ScoreChanger()
    {
        score += 5;
        scoreText.text = "Score: " + score.ToString();
        endScore.text = "Score: " + score.ToString();
    }

    //If fuel is collected than add 20
    //Then making sure that fuel does not go over 100
    //And then output too the UI
    public void FuelChanger()
    {
        fuel = fuel + 10;
        if (fuel > 100)
        {
            fuel = 100;
        }        
        fuelText.text = "Fuel: " + fuel.ToString();
    }
    //Pauses the game
    public void StartOff()
    {
        if (paused)
        {
            Time.timeScale = 1;
            startButton.SetActive(false);
        }
    }

    //This function is called above using modulo
    //This changes the speed variables to speed the game up
    public void SpeedUp()
    {
        RepeatingGroung groundScript = GameObject.Find("model_road_junction1_groundplane").GetComponent<RepeatingGroung>();
        RepeatingGroung groundScript2 = GameObject.Find("model_road_junction1_groundplane (1)").GetComponent<RepeatingGroung>();
        Background backgroundScript = GameObject.Find("Background_1").GetComponent<Background>();
        Background backgroundScript2 = GameObject.Find("model_road_blank").GetComponent<Background>();
        TruckController truckMover = GameObject.Find("model_vehicle_haytruck_0").GetComponent<TruckController>();

        groundScript.Road_Speed += 0.001f; //Mathf.Pow(score, 2) / 300;
        groundScript2.Road_Speed += 0.001f; //Mathf.Pow(score, 2) / 300;

       // backgroundScript.backgroundSpeed += 0.001f;
        backgroundScript2.roadSpeed += 0.5f;
        truckMover.Speed += 0.5f;
        birdSpeed += 0.5f;
        HouseSpeed -= 0.5f;


        //for (int i = 0; i < House.Length; i++)
        //{
        //    House[i].gameObject.GetComponent<HouseMover>().Speed -= 100f;
        //}

    }
}
