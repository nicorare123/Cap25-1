using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLaser : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("레이저 피격");
        }
    }
}
