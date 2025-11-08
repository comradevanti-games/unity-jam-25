using UnityEngine;

/// <summary>
/// Marks this game-object as a part which can attach to <see cref="PartDock"/>.
/// </summary>
[RequireComponent(typeof(FixedJoint))]
public class PartDocker : MonoBehaviour
{
    private FixedJoint attachJoint = null!;
    private PartDock? currentDock;

    /// <summary>
    /// The dock the part is currently attached to.
    /// </summary>
    public PartDock? CurrentDock
    {
        get => currentDock;
        private set
        {
            if (currentDock != null) currentDock.Undock(this);

            if (value != null)
                value.Dock(this);

            attachJoint.connectedBody = value != null ? value.Rigidbody : null;
            currentDock = value;
        }
    }

    public bool IsDocked => CurrentDock != null;


    private void OnCollisionEnter(Collision other)
    {
        if (IsDocked) return;

        if (other.gameObject.GetComponent<PartDock>() is not { } dock) return;

        CurrentDock = dock;
    }

    private void Awake()
    {
        attachJoint = GetComponent<FixedJoint>();
    }
}