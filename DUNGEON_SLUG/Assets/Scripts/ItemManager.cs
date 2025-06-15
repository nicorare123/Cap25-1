using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject SpwanTarget; //아이템 박스 넣기
    public GameObject SpwanPoint;
    public GameObject SpwanPoint1;
    public GameObject SpwanPoint2;
    public GameObject SpwanPoint3;
    public float currentTime = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldSpawn = Timer();

        if (shouldSpawn)
        {
            int randomIndex = Random.Range(0, 4); // 0~3 랜덤

            GameObject spawn = Instantiate(SpwanTarget);

            switch (randomIndex)
            {
                case 0:
                    spawn.transform.position = SpwanPoint.transform.position;
                    Debug.Log("아이템 박스 드랍-0");
                    break;
                case 1:
                    spawn.transform.position = SpwanPoint1.transform.position;
                    Debug.Log("아이템 박스 드랍-1");
                    break;
                case 2:
                    spawn.transform.position = SpwanPoint2.transform.position;
                    Debug.Log("아이템 박스 드랍-2");
                    break;
                case 3:
                    spawn.transform.position = SpwanPoint3.transform.position;
                    Debug.Log("아이템 박스 드랍-3");
                    break;
            }
        }
    }
    bool Timer()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 7.5f)
        {
            currentTime = 0f;
            return true;
        }

        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision) //충돌판정 명령어?
    {
        if (collision.gameObject.name == "Floor") //충돌해서 자폭하는 조건 
        {
            Destroy(gameObject, 2.0f); //2초 뒤에 사라짐
            Debug.Log("자폭"); //자폭 확인용
        }

    }
}
