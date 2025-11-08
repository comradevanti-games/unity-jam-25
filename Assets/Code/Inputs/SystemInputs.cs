using System;
using UnityEngine;

public class SystemInputs : MonoBehaviour {

    private void Awake() {
        FindAnyObjectByType<InputHandler>().QuitInput += OnQuitInputReceived;
    }

    private void OnQuitInputReceived(bool shouldQuit) {
        if (shouldQuit) {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

}