using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private float zDistance = 10f;
    [SerializeField] private Transform defaultTarget = null;

    private Transform followTransform = null;

    private bool IsAtTarget => followTransform != null &&
                               Mathf.Approximately(transform.position.x, followTransform.position.x) &&
                               Mathf.Approximately(transform.position.y, followTransform.position.y);

    private void Awake() {
        followTransform = defaultTarget;
    }

    public void SetFollowTarget(Transform target) => followTransform = target;

    private void LateUpdate() {

        if (!followTransform) {
            return;
        }

        if (IsAtTarget) {
            return;
        }

        Vector3 targetPosition = new(
            followTransform.position.x,
            followTransform.position.y,
            -zDistance
        );

        transform.position = targetPosition;

    }

}