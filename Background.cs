using UnityEngine;
using System.Collections;
/// <summary>
/// This script is simple to have a scrolling background
/// This works slightly different from the road as this moves the texture around the plane
/// Giving the illusion of movement
/// 
/// This script is used to have the effect of moving object across the screen
/// This gives the illusion that the player is moving along
/// 
/// Roughly above the speed of 80 will distort the players view of directional motion (It appears as if the road is going backwards)
/// 
/// </summary>
public class Background : MonoBehaviour {

    public float backgroundSpeed;
    public float roadSpeed;
    public bool road;
    public bool background;
    public bool ground;
    private Renderer rend;


    void Start ()
    {
        rend = GetComponent<Renderer>();
	}
	
	void Update ()
    {
        //This simply makes it so the texture moves along the plane
        //The texture wrap is repeated so that it will continue to loop
        if (background == true)
        {
            rend.material.mainTextureOffset = new Vector2(Time.time * backgroundSpeed, 0f);
        }

        if (road == true)
        {
            rend.material.mainTextureOffset = new Vector2(0, Time.time * roadSpeed);
        }

        if (ground == true)
        {
            rend.material.SetTextureOffset("_Blendtexture", new Vector2(Time.time * roadSpeed, 0));
            rend.material.SetTextureOffset("_Grasstexturered", new Vector2(Time.time * roadSpeed, 0));
            rend.material.SetTextureOffset("_Rocktexturegreen", new Vector2(Time.time * roadSpeed, 0));
            rend.material.SetTextureOffset("_Basetexture", new Vector2(Time.time * roadSpeed, 0));
            rend.material.SetTextureOffset("_r2t_mask", new Vector2(Time.time * roadSpeed, 0));
        }
    }
}
