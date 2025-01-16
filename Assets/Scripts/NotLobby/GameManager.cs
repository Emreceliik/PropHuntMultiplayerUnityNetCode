using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        // Oyuncu sahneye girdiğinde karakter prefab'ını instantiate et
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(9.5f,-2.75f,-6.23000002f), Quaternion.identity);
        }
    }
}
