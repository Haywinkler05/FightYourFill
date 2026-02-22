using UnityEngine;

public class Arrow : MonoBehaviour
{
    protected float lifeSpan { get; set; }
    protected float arrowSpeed { get; set; }
    protected float arrowGravity { get; set; }
    protected int arrowPierce { get; set; }
    protected float arrowDamage { get; set; }
    private Rigidbody rb;

    void FixedUpdate()
    {
        // Modify arrow velocity based on internal gravity component
        rb.linearVelocity += Vector3.down * arrowGravity * Time.fixedDeltaTime;

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Set default values for arrow
        lifeSpan = 4f;
        arrowSpeed = 10f;
        arrowGravity = 4.9f;
        arrowPierce = 0;
        arrowDamage = 6f;
        Destroy(gameObject, lifeSpan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }
}
