using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public GameObject flamePrefab;
    public float fireDelay = 1.2f;
    public float flameDuration = 1.0f;
    public float flameSpeed = 2.0f;

    private float fireTimer = 0f;
    private PlayerMove player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMove>();
        fireTimer = fireDelay;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && fireTimer >= fireDelay)
        {
            fireTimer = 0f;

            if (!player.UseAmmo()) return;

            Vector2 dir = player.GetFireDirection();
            Vector2 pos = (Vector2)transform.position + dir * (player.fireOffset + 2.0f);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameObject flame = Instantiate(flamePrefab, pos, rot);
            MyFlame flameScript = flame.GetComponent<MyFlame>();
            flameScript.direction = dir;
            flameScript.speed = flameSpeed;
            Destroy(flame, flameDuration);
        }
    }
}
