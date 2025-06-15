using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType { Normal, MachineGun, Laser, Rocket, Flame }
public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float jump = 1.0f;
    public Transform PlayerTr;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public float bulletSpeed = 6.0f;
    public float fireOffset = 0.5f;
    public float throwForce = 5.0f;
    public float throwUpwardForce = 6.0f;
    public bool holdUp = false;
    public bool holdDown = false;
    public WeaponType currentWeapon = WeaponType.Normal;
    private int ammoCount = 0;
    public GameObject pistolPrefab;
    public GameObject machineGunPrefab;
    public GameObject rocketLauncherPrefab;
    public GameObject laserGunPrefab;
    public GameObject flameThrowerPrefab;
    private GameObject currentWeaponPrefab;
    public Transform muzzleTransform;
    public int grenadeCount = 10;
    private Vector2 lastDirection = Vector2.right;

    Vector3 movement;
    Rigidbody2D rigid;
    bool isJumping = false;
    public bool isTitleScene = false;

    float horizontal;
    public Animator animator;

    public Text livesText;
    public Text grenadeText;
    public Image weaponIcon;
    public Text ammoText;

    public Sprite pistolIcon, machineGunIcon, laserIcon, rocketIcon, flameIcon;

    public int lives = 3;

    public SkinnedMeshRenderer[] skinnedMeshes;

    // Start is called before the first frame update
    void Start()
    {
        //skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();

        rigid = GetComponent<Rigidbody2D>();
        Transform found = transform.Find("FirePosition");
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
        if(isDead)
        {
            return;
        }
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
    }

    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

        Move();
        if (isJumping)
        {
            Jump();
        }
    }
    void UpdateLivesUI()
    {
        if (isTitleScene) return;
        livesText.text = "x " + lives.ToString();
    }
    private bool isDead = false;
    private bool isInvincible = false;

    public void TakeDamage()
    {
        if (isDead || isInvincible) return;

        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {
        isDead = true;
        animator.SetBool("IsDie", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        transform.position = Vector3.up * 1.5f;

        animator.SetBool("IsDie", false);
        isDead = false;

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float blinkDuration = 0.2f;
        float timer = 0f;

        while (timer < 3f)
        {
            SetRenderersAlpha(0.3f); // 반투명
            yield return new WaitForSeconds(blinkDuration);

            SetRenderersAlpha(1f);   // 원래대로
            yield return new WaitForSeconds(blinkDuration);

            timer += blinkDuration * 2;
        }

        SetRenderersAlpha(1f); // 최종 복원
        isInvincible = false;
    }

    void SetRenderersAlpha(float alpha)
    {
        foreach (var renderer in skinnedMeshes)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;

                    // 알파 블렌딩 활성화
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
            }
        }
    }
    void GameOver()
    {
        isDead = true;
        animator.SetBool("IsDie", true);
        GameManager.instance.GameOver();
    }
    public void GameClear()
    {
        isDead = true;
        GameManager.instance.GameClear();
    }

    void UpdateWeaponUI()
    {
        if (isTitleScene) return;
        grenadeText.text = "x " + grenadeCount.ToString();
        ammoText.text = currentWeapon == WeaponType.Normal ? "x Infinite" : "x " + ammoCount.ToString();

        switch (currentWeapon)
        {
            case WeaponType.Normal: weaponIcon.sprite = pistolIcon; break;
            case WeaponType.MachineGun: weaponIcon.sprite = machineGunIcon; break;
            case WeaponType.Laser: weaponIcon.sprite = laserIcon; break;
            case WeaponType.Rocket: weaponIcon.sprite = rocketIcon; break;
            case WeaponType.Flame: weaponIcon.sprite = flameIcon; break;
        }
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
        UpdateWeaponUI();
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
            UpdateWeaponUI();
            return;
        }
        DestroyCurrentWeapon();

        currentWeapon = newWeapon;

        switch (newWeapon)
        {
            case WeaponType.Normal:
                currentWeaponPrefab = Instantiate(pistolPrefab, transform.position, Quaternion.identity, transform);
                ammoCount = -1;
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
        UpdateWeaponUI();
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

        grenadeCount += 10;
        if (grenadeCount > 99) grenadeCount = 99;

        UpdateWeaponUI();
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

    void Jump()
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
        if (currentWeapon != WeaponType.Normal) return;

        Vector2 direction = GetFireDirection();
        Vector2 firePos = muzzleTransform.position;

        GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);
        MyBullet bulletScript = bullet.GetComponent<MyBullet>();
        bulletScript.direction = direction;
        bulletScript.speed = bulletSpeed;

    }
    void ThrowGrenade()
    {
        if (!isTitleScene && grenadeCount <= 0) return;

        if (!isTitleScene)
        {
            grenadeCount--;
            UpdateWeaponUI();
        }
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
