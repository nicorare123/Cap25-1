using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Boss : MonoBehaviour
{
    public float speed = 2.0f;

    Vector3 dir = Vector3.zero;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMove player = collision.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.PickupItem();
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "Ground") //충돌해서 자폭하는 조건 
        {
            Destroy(gameObject, 2.0f); //2초 뒤에 사라짐
            Debug.Log("자폭"); //자폭 확인용
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (speed * dir * Time.deltaTime);
    }
}
