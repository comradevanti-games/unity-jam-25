using UnityEngine;

/// <summary>
/// Keeps a reference to the parent cell part that this part is attached to.
/// </summary>
[RequireComponent(typeof(FixedJoint))]
public class CellParent : MonoBehaviour
{
    private FixedJoint attachJoint = null!;

    public CellPart? Part
    {
        get =>
            attachJoint.connectedBody is { } attached
                ? CellQ.CellPartOf(attached.gameObject)!
                : null;
        set =>
            attachJoint.connectedBody =
                value?.GameObject.GetComponent<Rigidbody>();
    }


    private void Awake()
    {
        attachJoint = GetComponent<FixedJoint>();
    }
}