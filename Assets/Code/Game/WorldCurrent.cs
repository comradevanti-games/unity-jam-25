using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldCurrent : MonoBehaviour {

    public int pixWidth;
    public int pixHeight;

    public float xOrg;
    public float yOrg;

    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    [SerializeField] private Renderer rend;
    [SerializeField] private float applyCurrentTimer = 0f;
    [SerializeField] private float currentForceLow = 0f;
    [SerializeField] private float currentForceHigh = 1f;

    private void Start() {
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
        StartCoroutine(ApplyCurrent());
    }

    private void CalcNoise() {

        for (float y = 0.0F; y < noiseTex.height; y++) {
            for (float x = 0.0F; x < noiseTex.width; x++) {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private void FixedUpdate() {

        //xOrg += 0.0001f;
        //yOrg += 0.0005f;

        //CalcNoise();

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