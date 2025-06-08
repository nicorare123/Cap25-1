using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float lifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMove player = collision.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.TakeDamage();
            }

            Debug.Log("ÇÇ°Ý");
            Destroy(gameObject);
        }
    }
}
