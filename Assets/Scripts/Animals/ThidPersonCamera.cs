using UnityEngine;
using Photon.Pun;

public class OrbitCamera : MonoBehaviourPun
{
    public Transform target; // Kameranın etrafında döneceği hedef (karakter)
    public float distance = 5.0f; // Kamera ile hedef arasındaki mesafe
    public float xSpeed = 120.0f; // Yatay dönüş hızını ayarlayın
    public float ySpeed = 120.0f; // Dikey dönüş hızını ayarlayın
    public float yMinLimit = 0f; // Dikey dönüş için minimum açı sınırı
    public float yMaxLimit = 80f; // Dikey dönüş için maksimum açı sınırı
    public float distanceMin = 1f; // Mesafenin minimum sınırı
    public float distanceMax = 10f; // Mesafenin maksimum sınırı


    private float x = 0.0f; // Yatay açı
    private float y = 0.0f; // Dikey açı

    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Mesafeyi sınırlandır
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);
    }

    void LateUpdate()
    {
        if (photonView.IsMine)
        {
        if (target)
        {
            // Mouse hareketini al
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // Dikey açı sınırlarını uygula
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            // Kamerayı hedefe göre konumlandır
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;

            // Mesafe değişimi
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * 5, distanceMin, distanceMax);
        }
        }

    }
}
