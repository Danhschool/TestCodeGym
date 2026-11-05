using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

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
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            Destroy(gameObject);
        }
    }
}

