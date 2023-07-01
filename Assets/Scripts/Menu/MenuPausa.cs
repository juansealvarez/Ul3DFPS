using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public static MenuPausa Instance { private set; get; }
    public GameObject menuPausa;
    public static bool isPaused = false;
    public GameObject mainUI;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        menuPausa.SetActive(false);
    }

    public void PausarJuego()
    {
        Debug.Log("Pausando juego...");
        PlayerController.mPlayerInput.SwitchCurrentActionMap("PauseMenu");
        menuPausa.SetActive(true);
        mainUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ReanudarJuego()
    {
        Debug.Log("Reanudando juego...");
        PlayerController.mPlayerInput.SwitchCurrentActionMap("Player");
        menuPausa.SetActive(false);
        mainUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void IrAlMenu()
    {
        Debug.Log("Regresando al menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
