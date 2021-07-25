using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class LoadOnActivation2 : MonoBehaviour
{
    
    void OnEnable()
    {

        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene(18, LoadSceneMode.Single);
    }
}
