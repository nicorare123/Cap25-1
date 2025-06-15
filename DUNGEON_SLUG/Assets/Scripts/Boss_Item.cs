using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Item : MonoBehaviour
{
    public float speed = 2.0f;

    Vector3 dir = Vector3.zero;
    public float gravityScale = 0.5f;
    private Rigidbody2D rb;
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
        else if (collision.gameObject.name == "Floor")
        {
            Destroy(gameObject);
            Debug.Log("ÀÚÆø"); //ÀÚÆø È®ÀÎ¿ë
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (speed * dir * Time.deltaTime);
    }
}
