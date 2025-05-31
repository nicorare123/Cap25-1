using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    Image barImage;
    public float MaxHealth = 10.0f;
    public float currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        barImage = GetComponent<Image>();
        currentHealth = MaxHealth;
        barImage.fillAmount = currentHealth / MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        barImage.fillAmount = currentHealth / MaxHealth;
    }
}
