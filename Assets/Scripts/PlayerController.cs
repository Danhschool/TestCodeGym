using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float boundaryPadding = 0.5f;
    [SerializeField] private Camera mainCamera;

    private NavMeshAgent agent;
    private InputAction moveAction;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.updateRotation = false;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (moveAction == null) return;

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector3 targetPosition = transform.position + moveDirection * (agent.speed * Time.deltaTime);
        Vector3 clampedPosition = ClampToCameraView(targetPosition);

        if (agent.isOnNavMesh)
        {
            agent.Move(clampedPosition - transform.position);
        }
        else
        {
            transform.position = clampedPosition;
        }
    }

    private Vector3 ClampToCameraView(Vector3 targetPosition)
    {
        if (mainCamera == null) return targetPosition;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(targetPosition);
        if (viewportPos.z < 0f) return targetPosition;

        float frustumHeight = mainCamera.orthographic
            ? mainCamera.orthographicSize * 2f
            : 2f * viewportPos.z * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * mainCamera.aspect;

        float marginX = frustumWidth > 0.001f ? Mathf.Clamp(boundaryPadding / frustumWidth, 0f, 0.49f) : 0f;
        float marginY = frustumHeight > 0.001f ? Mathf.Clamp(boundaryPadding / frustumHeight, 0f, 0.49f) : 0f;

        viewportPos.x = Mathf.Clamp(viewportPos.x, marginX, 1f - marginX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, marginY, 1f - marginY);

        Vector3 clampedWorldPos = mainCamera.ViewportToWorldPoint(viewportPos);
        clampedWorldPos.y = targetPosition.y;

        return clampedWorldPos;
    }
}
