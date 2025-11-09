using System;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private float yDistance = 10f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private Transform defaultTarget = null;

    private Transform followTransform = null;
    private Coroutine distanceChangeCoroutine = null;

    private bool IsAtTarget => followTransform != null &&
                               Mathf.Approximately(transform.position.x, followTransform.position.x) &&
                               Mathf.Approximately(transform.position.y, followTransform.position.y);

    private void Awake() {
        followTransform = defaultTarget;
    }

    public void SetFollowTarget(Transform target) => followTransform = target;

    public void SetCameraDistance(float newDistance) {
        if (distanceChangeCoroutine != null) {
            StopCoroutine(distanceChangeCoroutine);
        }
        
        distanceChangeCoroutine = StartCoroutine(AnimateDistance(newDistance, duration));
    }

    private IEnumerator AnimateDistance(float targetDistance, float duration) {
        float timeElapsed = 0f;
        float startDistance = yDistance;

        while (timeElapsed < duration) {
            float t = timeElapsed / duration;
            yDistance = Mathf.Lerp(startDistance, targetDistance, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        yDistance = targetDistance;
        distanceChangeCoroutine = null;
    }

    private void LateUpdate() {

        if (!followTransform) {
            return;
        }

        if (IsAtTarget) {
            return;
        }

        Vector3 targetPosition = new(
            followTransform.position.x,
            yDistance,
            followTransform.position.z
        );

        transform.position = targetPosition;

    }

}