using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        if (Timer())
        {
            GameObject Spwan = Instantiate(SpwanTarget);
            Spwan.transform.position = SpwanPoint.transform.position;
            Debug.Log("������ �ڽ� ���");
        }
        else if (Timer())
        {
            GameObject Spwan = Instantiate(SpwanTarget);
            Spwan.transform.position = SpwanPoint1.transform.position;
            Debug.Log("������ �ڽ� ���-1");
        }
        else if (Timer())
        {
            GameObject Spwan = Instantiate(SpwanTarget);
            Spwan.transform.position = SpwanPoint2.transform.position;
            Debug.Log("������ �ڽ� ���-2");
        }
        else if (Timer())
        {
            GameObject Spwan = Instantiate(SpwanTarget);
            Spwan.transform.position = SpwanPoint3.transform.position;
            Debug.Log("������ �ڽ� ���-3");
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
