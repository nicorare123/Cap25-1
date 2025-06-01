using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case "Map_1":
                    SceneManager.LoadScene("Map_2");
                    break;
                case "Map_2":
                    SceneManager.LoadScene("Map_3");
                    break;
                case "Map_3":
                    SceneManager.LoadScene("Map_4");
                    break;
                default:
                    Debug.Log("마지막 맵이거나 등록되지 않은 씬입니다.");
                    break;
            }
        }
    }
}
