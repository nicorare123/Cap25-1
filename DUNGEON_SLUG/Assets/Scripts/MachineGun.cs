using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 15.0f;
    public float fireDelay = 0.1f;

    private PlayerMove player;

    private float fireTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireDelay)
            {
                fireTimer = 0f;

                if (!player.UseAmmo()) return;

                Vector2 dir = player.GetFireDirection();
                Vector2 pos = (Vector2)transform.position + dir * player.fireOffset;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);

                GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                MyBullet bulletScript = bullet.GetComponent<MyBullet>();
                bulletScript.direction = dir;
                bulletScript.speed = bulletSpeed;
            }
        }
        else
        {
            fireTimer = fireDelay;
        }
    }
}
