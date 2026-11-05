using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField, Range(0f, 0.4f)] private float viewportPadding = 0.05f;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);
        while (true)
        {
            yield return wait;
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        if (itemPrefab == null || mainCamera == null) return;

        if (TryGetRandomPositionInView(out Vector3 position))
        {
            Instantiate(itemPrefab, position, Quaternion.identity);
        }
    }

    private bool TryGetRandomPositionInView(out Vector3 position)
    {
        Vector3 viewportPoint = new Vector3(
            Random.Range(viewportPadding, 1f - viewportPadding),
            Random.Range(viewportPadding, 1f - viewportPadding),
            0f);

        Ray ray = mainCamera.ViewportPointToRay(viewportPoint);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, spawnHeight, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            position = ray.GetPoint(distance);
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}

