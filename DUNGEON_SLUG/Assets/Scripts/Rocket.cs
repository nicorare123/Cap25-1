using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject rocketPrefab;
    public float rocketSpeed = 8.0f;
    public float fireDelay = 1.0f;

    private float fireTimer = 0f;
    private PlayerMove player;

    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
        fireTimer = fireDelay;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.J) && fireTimer >= fireDelay)
        {
            fireTimer = 0f;

            if (!player.UseAmmo()) return;

            Vector2 dir = player.GetFireDirection();
            Vector2 pos = (Vector2)transform.position + dir * player.fireOffset;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);

            GameObject rocket = Instantiate(rocketPrefab, pos, rotation);
            MyBullet rocketScript = rocket.GetComponent<MyBullet>();
            rocketScript.direction = dir;
            rocketScript.speed = rocketSpeed;
        }
    }
}
