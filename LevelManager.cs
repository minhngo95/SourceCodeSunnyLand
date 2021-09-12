using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Vector2 playerInitPosition;
    private void Start()
    {
        playerInitPosition = FindObjectOfType<KeyboardControll2>().transform.position;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //FindObjectOfType<KeyboardControll2>().ResetPlayer();
        //FindObjectOfType<KeyboardControll2>().transform.position = playerInitPosition;

    }
}
