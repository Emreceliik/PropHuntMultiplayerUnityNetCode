using UnityEngine;
using System.Collections;

public class RandomWalking : MonoBehaviour
{
    public float moveSpeed = 2f;                   // Karakterin hareket hızı
    public float minRotationSpeed = 50f;           // Minimum dönüş hızı
    public float maxRotationSpeed = 150f;          // Maksimum dönüş hızı
    public float rotationSmoothness = 0.2f;        // Dönüşün ne kadar pürüzsüz olacağı
    public float minWalkTime = 1f;                 // Minimum yürüme süresi
    public float maxWalkTime = 5f;                 // Maksimum yürüme süresi
    public float minWaitTime = 1f;                 // Minimum bekleme süresi
    public float maxWaitTime = 3f;                 // Maksimum bekleme süresi
    public float directionChangeInterval = 0.5f;   // Dönüş yönünün değişme aralığı

    private Animator animator;                     // Animator bileşeni
    private Rigidbody rb;                          // Rigidbody bileşeni
    private bool isWalking = false;                // Yürüyüp yürümediğini belirler
    private float currentRotationSpeed;            // Mevcut dönüş hızı

    void Start()
    {
        animator = GetComponent<Animator>();  // Animator bileşenini alır
        rb = GetComponent<Rigidbody>();       // Rigidbody bileşenini alır
        StartCoroutine(WalkAndWait());        // WalkAndWait coroutine'ini başlatır
    }

    IEnumerator WalkAndWait()
    {
        while (true)
        {
            // Yürüme süresini rastgele belirle
            float walkDuration = Random.Range(minWalkTime, maxWalkTime);

            // Rastgele bir süre yürüme
            isWalking = true;
            animator.SetBool("IsWalking", true);

            for (float t = 0; t < walkDuration; t += Time.deltaTime)
            {
                Vector3 move = transform.forward * moveSpeed;
                rb.velocity = move;

                // Her directionChangeInterval süresinde yeni bir dönüş hızı belirle
                if (t % directionChangeInterval < Time.deltaTime)
                {
                    currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
                }

                // Dönüşü pürüzsüz hale getir
                transform.Rotate(Vector3.up, Mathf.Lerp(0, currentRotationSpeed, rotationSmoothness) * Time.deltaTime);

                yield return null;
            }

            // Yürümeyi durdur ve bekle
            isWalking = false;
            animator.SetBool("IsWalking", false);
            rb.velocity = Vector3.zero;  // Hızı sıfırla, karakter durur

            // Bekleme süresini rastgele belirle
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);
        }
    }
}
