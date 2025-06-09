using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Item : MonoBehaviour
{
    public float speed = 5.0f;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (speed * dir * Time.deltaTime * 1.0f);
    }
    private void OnCollisionEnter2D(Collision2D collision) //�浹���� ��ɾ�?
    {
        if (collision.gameObject.name == "Floor") //�浹�ؼ� �����ϴ� ���� 
        {
            Destroy(gameObject, 3.5f);
            Debug.Log("����"); //���� Ȯ�ο�
        }

    }
}
