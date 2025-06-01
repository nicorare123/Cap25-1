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
            Vector2 pos = muzzleTransform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject rocket = Instantiate(rocketPrefab, pos, rotation);
            MyRocket rocketScript = rocket.GetComponent<MyRocket>();
            rocketScript.direction = dir;
            rocketScript.speed = rocketSpeed;
        }
    }
}
