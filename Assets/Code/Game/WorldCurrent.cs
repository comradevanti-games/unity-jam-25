using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldCurrent : MonoBehaviour {

    [SerializeField] private float applyCurrentTimer = 0f;
    [SerializeField] private float currentForceLow = 0f;
    [SerializeField] private float currentForceHigh = 1f;

    private void Start() {
        StartCoroutine(ApplyCurrent());
    }

    private IEnumerator ApplyCurrent() {
        while (true) {
            var allRigidbodies = FindObjectsByType<Rigidbody>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);

            if (allRigidbodies != null) {
                foreach (Rigidbody rb in allRigidbodies) {
                    rb.AddForce(new Vector3(Random.Range(currentForceLow, currentForceHigh), 0,
                        Random.Range(currentForceLow, currentForceHigh)));
                }
            }

            yield return new WaitForSeconds(applyCurrentTimer);
        }

    }

}