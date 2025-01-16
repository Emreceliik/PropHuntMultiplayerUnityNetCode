using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber];
        GameObject playerTospawnn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        if (PhotonNetwork.IsConnected)
        {
        PhotonNetwork.Instantiate(playerTospawnn.name, spawnPoint.position, Quaternion.identity);
    }
        

    }
}
