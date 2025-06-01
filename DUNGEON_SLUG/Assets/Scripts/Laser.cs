using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserPrefab;     
    public float fireOffset = 0.5f;      
    public float ammoUsage = 0.1f;

    private PlayerMove player;
    private float ammoTimer = 0f;
    private GameObject activeLaser;

    public float laserOffset = 15.0f;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            ammoTimer += Time.deltaTime;

            if (ammoTimer >= ammoUsage)
            {
                ammoTimer = 0f;
                if (!player.UseAmmo())
                {
                    DestroyLaser();
                    return;
                }
            }
            Vector2 dir = player.GetFireDirection();
            Vector2 spawnPos = (Vector2)muzzleTransform.position + dir * laserOffset;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            if (activeLaser == null)
            {
                activeLaser = Instantiate(laserPrefab, spawnPos, rot, transform);
            }
            else
            {
                activeLaser.transform.position = spawnPos;
                activeLaser.transform.rotation = rot;
            }
        }
        else
        {
            DestroyLaser();
            ammoTimer = ammoUsage;
        }
    }

    void DestroyLaser()
    {
        if(activeLaser != null)
        {
            Destroy(activeLaser);
            activeLaser = null;
        }
    }
}
