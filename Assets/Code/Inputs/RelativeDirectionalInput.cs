using UnityEngine;
using UnityEngine.Events;

public class RelativeDirectionalInput : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler = null!;
    [SerializeField] private Transform? relativeTarget;

    public UnityEvent<Vector3> inputReceived = new UnityEvent<Vector3>();

    private void OnAbsoluteInput(Vector2 input)
    {
        var absolute = new Vector3(input.x, 0, input.y);
        var relative = relativeTarget
            ? relativeTarget.InverseTransformDirection(absolute)
            : absolute;
        inputReceived.Invoke(relative);
    }

    private void OnDisable()
    {
        inputHandler.MovementInputHandled -= OnAbsoluteInput;
    }

    private void OnEnable()
    {
        inputHandler.MovementInputHandled += OnAbsoluteInput;
    }
}