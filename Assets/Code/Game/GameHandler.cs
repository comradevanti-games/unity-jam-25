using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public event Action<bool>? GameInitialized;

    private void Start() {
        GameInitialized?.Invoke(true);
        Debug.Log(FindAnyObjectByType<World>().GetRandomWorldPoint());
    }

}