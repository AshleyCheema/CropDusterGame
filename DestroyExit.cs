using UnityEngine;
using System.Collections;

/// <summary>
/// This script was created so that if anything left the box collider would be destoryed
/// This is done so objects are not left in the game once they've past through
/// As they will no longer be visible on the screen
/// This also frees up space
/// </summary>
public class DestroyExit : MonoBehaviour 
{
    //Destorys any objects that exit the box collider
	void OnTriggerExit(Collider other)
	{
		Destroy(other.gameObject);
	}
}

