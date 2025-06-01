using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum WeaponType { Normal, MachineGun, Laser, Rocket, Flame }

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float jump = 11.0f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float fireOffset = 0.5f;

    public GameObject grenadePrefab;
    public float throwForce = 5.0f;
    public float throwUpwardForce = 6.0f;

    public bool holdUp = false;
    public bool holdDown = false;
    float horizontal;

    private Vector2 lastDirection = Vector2.right;

    Vector3 movement;
    Rigidbody2D rigid;
    bool isJumping = false;

    public Animator animator;
    private bool facingRight = true;

    public WeaponType currentWeapon = WeaponType.Normal;
    private int ammoCount = 0;
    public GameObject pistolPrefab;
    public GameObject machineGunPrefab;
    public GameObject rocketLauncherPrefab;
    public GameObject laserGunPrefab;
    public GameObject flameThrowerPrefab;
    private GameObject currentWeaponPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && IsGrounded())
        {
            isJumping = true;
        }

        holdUp = Input.GetKey(KeyCode.W);
        holdDown = Input.GetKey(KeyCode.S) && !IsGrounded();
        if (Input.GetKeyDown(KeyCode.J))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ThrowGrenade();
        }

        animator.SetFloat("Speed", horizontal);
        animator.SetBool("IsGrounded", IsGrounded());


        Debug.Log(holdUp);
        Debug.Log(holdDown);
    }

    private void FixedUpdate()
    {
        Move();
        if(isJumping)
        {
            Jump();
        }
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        movement = new Vector3(horizontal, 0, 0).normalized;

        if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            lastDirection = Vector2.right;
        }
        else if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            lastDirection = Vector2.left;
        }

        transform.position += movement * speed * Time.deltaTime;
    }

    public void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        isJumping = false;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector2.down * 0.2f, Color.red);

        return hit.collider != null;
    }
    
    void Fire()
    {
        Vector2 direction = lastDirection;

        if (holdUp)
        {
            direction = Vector2.up;
        }
        else if (holdDown)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = lastDirection;
        }

        Vector2 firePos = (Vector2)transform.position + direction * fireOffset;

        GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);

        MyBullet mybulletScript = bullet.GetComponent<MyBullet>();
        mybulletScript.direction = direction;
        mybulletScript.speed = bulletSpeed;
    }

    public Vector2 GetFireDirection()
    {
        if (holdUp) return Vector2.up;
        if (holdDown) return Vector2.down;
        return lastDirection;

    }

    public bool UseAmmo()
    {
        if (currentWeapon == WeaponType.Normal) return true;

        ammoCount--;
        if (ammoCount <= 0)
        {
            DestroyCurrentWeapon();
            ChangeWeapon(WeaponType.Normal);
            return false;
        }
        return true;
    }

    void ChangeWeapon(WeaponType newWeapon)
    {
        if (currentWeapon == newWeapon)
        {
            switch (newWeapon)
            {
                case WeaponType.MachineGun:
                    ammoCount += 200;
                    break;
                case WeaponType.Laser:
                    ammoCount += 100;
                    break;
                case WeaponType.Rocket:
                    ammoCount += 50;
                    break;
                case WeaponType.Flame:
                    ammoCount += 30;
                    break;
            }
            Debug.Log("장탄 추가: " + ammoCount);
            return;
        }
        DestroyCurrentWeapon();

        currentWeapon = newWeapon;

        switch (newWeapon)
        {
            case WeaponType.Normal:
                currentWeaponPrefab = Instantiate(pistolPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = -1; // 무한
                break;
            case WeaponType.MachineGun:
                currentWeaponPrefab = Instantiate(machineGunPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = 200;
                break;
            case WeaponType.Laser:
                currentWeaponPrefab = Instantiate(laserGunPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = 100;
                break;
            case WeaponType.Rocket:
                currentWeaponPrefab = Instantiate(rocketLauncherPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = 50;
                break;
            case WeaponType.Flame:
                currentWeaponPrefab = Instantiate(flameThrowerPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = 30;
                break;
        }
        Debug.Log("무기 변경 : " + currentWeapon + ammoCount + "발");
    }

    void DestroyCurrentWeapon()
    {
        if (currentWeaponPrefab != null)
        {
            Destroy(currentWeaponPrefab);
            currentWeaponPrefab = null;
        }
    }
    public void PickupItem()
    {
        WeaponType[] weapons = new WeaponType[]
        {
            WeaponType.MachineGun,
            WeaponType.Laser,
            WeaponType.Rocket,
            WeaponType.Flame
        };

        WeaponType randomWeapon = weapons[Random.Range(0, weapons.Length)];
        ChangeWeapon(randomWeapon);
    }

    
    void ThrowGrenade()
    {
        Vector2 spawnPos = (Vector2)transform.position + lastDirection * 0.5f + Vector2.up * 0.5f;
        GameObject grenade = Instantiate(grenadePrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 force = lastDirection * throwForce + Vector2.up * throwUpwardForce;
            rb.AddForce(force, ForceMode2D.Impulse);

            float dir = (lastDirection.x > 0) ? -500f : 500f;
            rb.AddTorque(dir, ForceMode2D.Force);
        }
    }
}
