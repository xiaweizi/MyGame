using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public static GameManager instance;

    public bool isGameOver;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        isGameOver = playerController.isDeath;
        UIManager.instance.GameOverUI(isGameOver);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
