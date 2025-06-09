using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject SpwanTarget; //아이템 박스 넣기
    public GameObject SpwanPoint;
    public float currentTime = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer())
        {
            GameObject Spwan = Instantiate(SpwanTarget);
            Spwan.transform.position = SpwanPoint.transform.position;
            Debug.Log("아이템 박스 드랍");
        }
    }
    bool Timer()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 3.5f)
        {
            currentTime = 0f;
            return true;
        }

        return false;
    }
}
