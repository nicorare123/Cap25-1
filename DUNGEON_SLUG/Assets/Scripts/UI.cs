using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public TextMeshProUGUI AmmoCount;
    public TextMeshProUGUI GrenadeCount;

    public int score;
    public int ammo;
    public int grenade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = "Score : " + score.ToString();
        AmmoCount.text = "AmmoCount\n \n" + ammo.ToString();
        GrenadeCount.text = "Grenade\n \n" + grenade.ToString();
    }
}
