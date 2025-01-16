using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimalMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f; // Hareket hızı
    public float mouseSensitivity = 2f; // Mouse hassasiyeti
    public float jumpForce = 5f; // Zıplama gücü
    public float rotationSpeed = 10f; // Dönüş hızını kontrol eden değişken
    private Rigidbody rb; // Rigidbody bileşeni
    private Animator animator; // Animator bileşeni
    private PhotonView view;

    private bool isGrounded; // Karada olup olmadığını kontrol eder

    void Start()
    {
        if (!photonView.IsMine) 
        {
            this.enabled = false; // Bu bileşeni devre dışı bırak
            return;
        }

        rb = GetComponent<Rigidbody>(); // Rigidbody bileşenini al
        view = GetComponent<PhotonView>();

        // Aktif olan Animator'ü al
        SetActiveAnimator(); 
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Kamera rotasını güncelle
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Hareket
            float horizontal = Input.GetAxis("Horizontal"); // Yatay eksende giriş
            float vertical = Input.GetAxis("Vertical"); // Düşey eksende giriş

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            Vector3 move = moveDirection * moveSpeed; // Hareket vektörü
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

            // Karakterin hareket ettiği yöne doğru dönmesi
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

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

    public void SetActiveAnimator()
    {
        // Objenin altındaki aktif olan objenin Animator bileşenini al
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                animator = child.GetComponent<Animator>();

                if (animator != null)
                {
                    break;
                }
            }
        }
    }
}
