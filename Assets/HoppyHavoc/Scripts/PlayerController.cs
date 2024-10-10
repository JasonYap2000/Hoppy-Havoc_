using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration = 10.0f; // Acceleration rate
    public float maxSpeed = 5.0f; // Maximum speed
    public float rotationSpeed = 100.0f; // Rotation speed
    public float jumpForce = 7.0f; // Jump force
    private Rigidbody rb; // Rigidbody component
    private Animator animator; // Animator component
    private Vector3 currentVelocity; // Current velocity
    [SerializeField]
    private bool isGrounded; // Ground check flag
    public Transform CameraRotateAxis;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
            CameraRotateAxis.Rotate(Vector3.left * mouseY * rotationSpeed * Time.deltaTime);

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 targetVelocity = new Vector3(horizontalInput, 0, verticalInput).normalized * maxSpeed;
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
            Vector3 movement = transform.TransformDirection(currentVelocity) * Time.deltaTime;

            rb.MovePosition(rb.position + movement);

            animator.SetFloat("Velocity", currentVelocity.magnitude/maxSpeed);

            if (Input.GetButton("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}