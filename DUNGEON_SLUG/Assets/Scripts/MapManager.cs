using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string[] sceneNames;

    // Start is called before the first frame update
    void Start()
    {
        int randomNum = Random.Range(0, sceneNames.Length);
        string selectedScene = sceneNames[randomNum];

        SceneManager.LoadScene(selectedScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
