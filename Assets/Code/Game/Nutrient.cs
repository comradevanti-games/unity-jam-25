using System;
using UnityEngine;

public class Nutrient : MonoBehaviour {

    public event Action<Nutrient>? Consumed;

    public float energy = 2f;

    private void OnTriggerEnter(Collider other) {
        Consumed?.Invoke(this);
    }

}