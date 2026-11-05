using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int scoreValue = 1;

    private bool collected;

    private void Awake()
    {
        if (!TryGetComponent(out Rigidbody rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (other.GetComponentInParent<PlayerController>() == null) return;

        collected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        Debug.Log("Player collected an item: " + GameManager.Instance.Score);

        Destroy(gameObject);
    }
}

