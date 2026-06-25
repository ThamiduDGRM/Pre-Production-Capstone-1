using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeGame : MonoBehaviour
{
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Escape.performed += ctx => ExitGame();
    }

    private void OnDisable()
    {
        controls.Player.Escape.performed -= ctx => ExitGame();
        controls.Disable();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}


