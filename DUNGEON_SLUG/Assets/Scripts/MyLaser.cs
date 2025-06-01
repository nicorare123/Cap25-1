using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLaser : MonoBehaviour
{
    public float damageInterval = 0.3f;
    private float damageTimer = 0f;

    private void Update()
    {
        damageTimer += Time.deltaTime;
    }
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && damageTimer >= damageInterval)
        {
            damageTimer = 0f;

            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(5);
            }
        }
    }
}
