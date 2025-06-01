using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float lifeTime = 5f;

    public GameObject HitEffect;
    public AudioSource hitSound;
    public AudioClip hitSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(hitSoundClip, collision.transform.position);

        if (collision.CompareTag("Enemy"))
        {
            HitPos(collision.transform.position);

            Debug.Log("ÇÇ°Ý");
        }

        Destroy(gameObject);
    }

    private void HitPos(Vector3 hitPos)
    {
        GameObject effect = Instantiate(HitEffect, hitPos, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
