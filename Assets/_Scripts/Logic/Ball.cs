using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Movement")]
    [SerializeField] private float ballLaunchSpeed;
    [SerializeField] private float minBallBounceBackSpeed;
    [SerializeField] private float maxBallBounceBackSpeed;
    [SerializeField] private float minYVelocity = 0.05f;
    [Header("References")]
    [SerializeField] private Transform ballAnchor;
    [SerializeField] private Rigidbody rb;

    private bool isBallActive;
    //Had an issue where ball would get stuck - no y velocity, infinately bouncing left and right
    private void Update()
    {
        // Check if the Y component of velocity is nearly zero but ball is still moving
        if (Mathf.Abs(rb.linearVelocity.y) < 0.001f && rb.linearVelocity.magnitude > 0.1f)
        {
            // Add a tiny Y component in a random direction
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = minYVelocity * Mathf.Sign(Random.value - 0.5f);
            rb.linearVelocity = currentVelocity;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Paddle"))
        {
            // Was easier to add it into here
            AudioManager.Instance.PlayPaddleHitSound();
            // _______

            Vector3 directionToFire = (transform.position - other.transform.position).normalized;
            float angleOfContact = Vector3.Angle(transform.forward, directionToFire);
            float returnSpeed = Mathf.Lerp(minBallBounceBackSpeed, maxBallBounceBackSpeed, angleOfContact / 90f);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(directionToFire * returnSpeed, ForceMode.Impulse);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            // For wall sound
            AudioManager.Instance.PlayWallHitSound();
        }
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        transform.parent = ballAnchor;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        isBallActive = false;
    }

    public void FireBall()
    {
        if (isBallActive) return;
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * ballLaunchSpeed, ForceMode.Impulse);
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        isBallActive = true;
    }
}
