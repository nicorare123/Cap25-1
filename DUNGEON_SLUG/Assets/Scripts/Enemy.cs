using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Melee, Ranged, Elite }
public enum EnemyState { Idle, Chase, MeleeAttack, Shoot }
public enum EliteState { Triple, Spread, Rain, Sweep, RandomSpread, Wait }
public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyState currentState;
    public EliteState eliteState = EliteState.Wait;

    public Transform Player;
    public GameObject bulletPrefab;

    public float moveSpeed = 3.0f;
    public float meleeAttackRange = 2.2f;
    public float shootRange = 8.0f;
    public float verticalRange = 1.2f;
    public float bulletSpeed = 10.0f;
    public float shootCoolDown = 1.2f;
    private float shootTimer = 0f;
    private float delayTimer = 0f;

    private bool hasEntered = false;
    private bool isAttacking = false;

    private void Start()
    {
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Player = playerObj.transform;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Melee:
                MeleeEnemy();
                break;
            case EnemyType.Ranged:
                RangedEnemy();
                break;
            case EnemyType.Elite:
                EliteEnemy();
                break;
        }
    }

    void MeleeEnemy()
    {
        float yDiff = Mathf.Abs(transform.position.y - Player.position.y);
        float xDiff = Mathf.Abs(transform.position.x - Player.position.x);

        if (yDiff > verticalRange)
        {
            currentState = EnemyState.Idle;
            Idle();
            return;
        }

        if (xDiff <= meleeAttackRange)
        {
            currentState = EnemyState.MeleeAttack;
            MeleeAttack();
        }
        else
        {
            currentState = EnemyState.Chase;
            Chase();
        }
    }

    void RangedEnemy()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        float yDiff = Player.position.y - transform.position.y;
        float xDiff = Mathf.Abs(Player.position.x - transform.position.x);
        float absYDiff = Mathf.Abs(yDiff);

        float xRange = 1.0f;

        if (distance <= meleeAttackRange && absYDiff < verticalRange)
        {
            currentState = EnemyState.MeleeAttack;
            MeleeAttack();
        }
        else if (distance <= shootRange)
        {
            if (absYDiff < verticalRange)
            {
                currentState = EnemyState.Shoot;
                Shoot();
            }
            else if (yDiff > verticalRange && xDiff < xRange)
            {
                currentState = EnemyState.Shoot;
                ShootUp();
            }
            else if (yDiff < -verticalRange)
            {
                currentState = EnemyState.Idle;
                Idle();
            }
        }
        else
        {
            currentState = EnemyState.Chase;
            Chase();
        }
    }

    void EliteEnemy()
    {
        float attackDelay = 1.0f;
        float distance = Vector2.Distance(transform.position, Player.position);

        if (!hasEntered && distance > shootRange)
        {
            currentState = EnemyState.Chase;
            Chase();
            return;
        }

        hasEntered = true;
        currentState = EnemyState.Shoot;

        if (eliteState == EliteState.Wait)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= attackDelay)
            {
                isAttacking = false;
                eliteState = (EliteState)Random.Range(0, 5);
                shootTimer = 0.0f;
                delayTimer = 0.0f;
            }
            return;
        }

        if (!isAttacking)
        {
            isAttacking = true;
            shootTimer = 0;
            Debug.Log("현재 패턴 : " + eliteState);
        }

        switch (eliteState)
        {
            case EliteState.Triple:
                TripleShoot();
                break;
            case EliteState.Spread:
                SpreadShoot();
                break;
            case EliteState.Rain:
                RainShoot();
                break;
            case EliteState.Sweep:
                SweepShoot();
                break;
            case EliteState.RandomSpread:
                RandomSpreadShoot();
                break;
        }
    }

    int tripleCount = 0;
    float tripleDelay = 0.3f;
    void TripleShoot()
    {
        shootTimer += Time.deltaTime;

        if (tripleCount < 3 && shootTimer >= tripleDelay)
        {
            shootTimer = 0;
            tripleCount++;

            Vector2 dir = (Player.position - transform.position).normalized;
            Vector2 pos = (Vector2)transform.position + dir * 0.5f;
            FireBullet(pos, dir);
        }

        if (tripleCount >= 3)
        {
            tripleCount = 0;
            eliteState = EliteState.Wait;
            shootTimer = 0f;
        }
    }

    int spreadCount = 0;
    float spreadDelay = 0.3f;
    bool spreadFired = false;
    void SpreadShoot()
    {
        shootTimer += Time.deltaTime;
        if (!spreadFired && shootTimer >= spreadDelay)
        {
            shootTimer = 0;
            spreadCount++;

            for (int i = -90; i <= 90; i += 15)
            {
                Vector2 dir = Quaternion.Euler(0, 0, i) * Vector2.up;
                FireBullet(transform.position, dir);
            }

            if (spreadCount >= 3)
            {
                spreadCount = 0;
                spreadFired = false;
                eliteState = EliteState.Wait;
                shootTimer = 0f;
            }
        }

    }

    int rainCount = 0;
    float rainDelay = 0.3f;

    void RainShoot()
    {
        shootTimer += Time.deltaTime;

        if (rainCount < 25 && shootTimer >= rainDelay)
        {
            shootTimer = 0;
            rainCount++;

            float x = Random.Range(Player.position.x - 7f, Player.position.x + 7f);
            float y = Player.position.y + 10f;
            Vector2 spawnPos = new Vector2(x, y);

            FireBullet(spawnPos, Vector2.down);
        }

        if (rainCount >= 25)
        {
            rainCount = 0;
            eliteState = EliteState.Wait;
            shootTimer = 0f;
        }
    }

    int sweepCount = 0;
    float sweepDelay = 0.3f;
    void SweepShoot()
    {
        shootTimer += Time.deltaTime;

        if (sweepCount < 25 && shootTimer >= sweepDelay)
        {
            shootTimer = 0;
            sweepCount++;

            float y = Random.Range(Player.position.y, Player.position.y + 7f);

            bool fromLeft = Random.Range(0, 2) == 0;
            Vector2 spawnPos;
            Vector2 direction;

            if (fromLeft)
            {
                spawnPos = new Vector2(Player.position.x - 10f, y);
                direction = Vector2.right;
            }
            else
            {
                spawnPos = new Vector2(Player.position.x + 10f, y);
                direction = Vector2.left;
            }

            FireBullet(spawnPos, direction);
        }

        if (sweepCount >= 25)
        {
            sweepCount = 0;
            eliteState = EliteState.Wait;
            shootTimer = 0f;
        }
    }

    int randSpreadCount = 0;
    float randSpreadDelay = 0.3f;

    void RandomSpreadShoot()
    {
        shootTimer += Time.deltaTime;

        if (randSpreadCount < 25 && shootTimer >= randSpreadDelay)
        {
            shootTimer = 0;
            randSpreadCount++;

            float angle = Random.Range(-90f, 90f);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            FireBullet(transform.position, dir);
        }

        if (randSpreadCount >= 25)
        {
            randSpreadCount = 0;
            eliteState = EliteState.Wait;
            shootTimer = 0f;
        }
    }


    void Chase()
    {
        Vector2 targetPos = new Vector2(Player.position.x, transform.position.y);
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void Idle()
    {
        Debug.Log("Wait");
    }

    void MeleeAttack()
    {
        Debug.Log("Melee Attack");
    }

    void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCoolDown)
        {
            shootTimer = 0;

            Vector2 direction = (Player.position.x < transform.position.x) ? Vector2.left : Vector2.right;
            Vector2 firePos = (Vector2)transform.position + direction * 0.5f;
            FireBullet(firePos, direction);
        }
    }

    void ShootUp()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCoolDown)
        {
            shootTimer = 0;

            Vector2 direction = Vector2.up;
            Vector2 firePos = (Vector2)transform.position + direction * 0.5f;
            FireBullet(firePos, direction);
        }
    }

    void FireBullet(Vector2 firePos, Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.direction = direction;
        bulletScript.speed = bulletSpeed;
    }
}
