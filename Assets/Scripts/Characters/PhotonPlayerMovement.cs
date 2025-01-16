using Photon.Pun;
using UnityEngine;

public class PhotonPlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f; // Hareket hızı
    public float mouseSensitivity = 2f; // Mouse hassasiyeti
    public float jumpForce = 5f; // Zıplama gücü

    public Transform playerCamera; // Kamera transformu

    private Rigidbody rb; // Rigidbody bileşeni
    private float xRotation = 0f; // X ekseninde döndürme açısı
    private Animator animator; // Animator bileşeni
    PhotonView view;

    private bool isGrounded; // Karada olup olmadığını kontrol eder

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>(); // Rigidbody bileşenini al
        animator = GetComponent<Animator>(); // Animator bileşenini al
        //Cursor.lockState = CursorLockMode.Locked; // Fareyi ekranın ortasına kilitle
        //Cursor.visible = false; // Fare imlecini gizle
    }

    void Update()
    {
        if (view.IsMine)
        {
            // Kamera rotasını güncelle
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            // Hareket
            float horizontal = Input.GetAxis("Horizontal"); // Yatay eksende giriş
            float vertical = Input.GetAxis("Vertical"); // Düşey eksende giriş

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            Vector3 move = moveDirection * moveSpeed ; // Hareket vektörü
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

            // Animasyonları güncelle
            bool isWalking = horizontal != 0 || vertical != 0;
            animator.SetBool("IsWalking", isWalking && isGrounded); // Yalnızca yerdeyken yürüyüş animasyonunu oynat

            // Zıplama
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; // Hemen havada olduğumuzu belirleyelim
            }

            animator.SetBool("IsJumping", !isGrounded); // Havada olduğumuzu belirlemek için zıplama animasyonunu ayarlayın
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Karaya temas ediyorsa isGrounded true yap
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Karadan ayrılırsa isGrounded false yap
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
