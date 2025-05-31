using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFlame : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    // Start is called before the first frame update
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("레이저 피격");
        }
    }
}
