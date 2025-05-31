using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 6.0f;
    public float fireOffset = 0.5f;

    private PlayerMove player;

    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector2 direction = player.GetFireDirection();
            Vector2 firePos = (Vector2)transform.position + direction * fireOffset;

            GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);
            MyBullet bulletScript = bullet.GetComponent<MyBullet>();
            bulletScript.direction = direction;
            bulletScript.speed = bulletSpeed;
        }
    }
}
