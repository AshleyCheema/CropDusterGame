using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

	public void ResetMainLevel()
    {
        SceneManager.LoadScene(0);
    }
}
