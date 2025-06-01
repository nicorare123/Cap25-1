using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 6.0f;
    public float fireOffset = 0.5f;

    private PlayerMove player;

    public Transform muzzleTransform;
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
        Transform found = player.transform.Find("FirePosition");
        if (found != null)
        {
            muzzleTransform = found;
        }
        else
        {
            Debug.LogWarning("FirePosition 오브젝트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector2 direction = player.GetFireDirection();
            Vector2 firePos = muzzleTransform.position;

            GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);
            MyBullet bulletScript = bullet.GetComponent<MyBullet>();
            bulletScript.direction = direction;
            bulletScript.speed = bulletSpeed;
        }
    }
}
