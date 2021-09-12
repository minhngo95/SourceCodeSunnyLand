using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image FillBar;

    public float health;

    public void Losehealth(int value)
    {
        if (health <= 0)
            return;
        health -= value;

        FillBar.fillAmount = health / 100;
        if(health <= 0)
        {
            FindObjectOfType<KeyboardControll2>().Die();
           
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        
            Losehealth(25);
        
    }
}
