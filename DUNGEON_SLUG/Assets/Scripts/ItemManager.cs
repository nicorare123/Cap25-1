using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject SpwanTarget; //������ �ڽ� �ֱ�
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
            int randomIndex = Random.Range(0, 4); // 0~3 ����

            GameObject spawn = Instantiate(SpwanTarget);

            switch (randomIndex)
            {
                case 0:
                    spawn.transform.position = SpwanPoint.transform.position;
                    Debug.Log("������ �ڽ� ���-0");
                    break;
                case 1:
                    spawn.transform.position = SpwanPoint1.transform.position;
                    Debug.Log("������ �ڽ� ���-1");
                    break;
                case 2:
                    spawn.transform.position = SpwanPoint2.transform.position;
                    Debug.Log("������ �ڽ� ���-2");
                    break;
                case 3:
                    spawn.transform.position = SpwanPoint3.transform.position;
                    Debug.Log("������ �ڽ� ���-3");
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
    private void OnCollisionEnter2D(Collision2D collision) //�浹���� ��ɾ�?
    {
        if (collision.gameObject.name == "Floor") //�浹�ؼ� �����ϴ� ���� 
        {
            Destroy(gameObject, 2.0f); //2�� �ڿ� �����
            Debug.Log("����"); //���� Ȯ�ο�
        }

    }
}
