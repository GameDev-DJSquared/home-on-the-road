using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] Slider healthSlider;
    

    Collider col;

    public int health, healthI = 100;
    public int damage = 20;

    public float invulTime = 0, invulTimeI = 3;
    bool invulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        health = healthI;
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = Mathf.Clamp((float)health / healthI, 0, 1);

        if (invulTime > 0)
        {
            healthSlider.fillRect.GetComponent<Image>().color = Color.white;
            invulTime -= Time.deltaTime;
            if (invulTime <= 0)
            {
                healthSlider.fillRect.GetComponent<Image>().color = new Color(28 / 255f, 161 / 255f, 57 / 255f); 

                invulTime = 0;
                invulnerable = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!invulnerable && other.gameObject.layer == 7)
        {
            health -= damage;
            if(health <= 0)
            {
                GameManager.instance.FinishGame(true);
            }
            invulTime = invulTimeI;
            invulnerable = true;

        }
    }

}
