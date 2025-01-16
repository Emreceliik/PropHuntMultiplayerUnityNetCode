using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnecToServer : MonoBehaviourPunCallbacks
{
    public InputField userNameInput;
    public Text buttontext;
    // Start is called before the first frame update
    public void OnClickConnect()
    {
        if(userNameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = userNameInput.text;
            buttontext.text = "Connecting";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

        }
        
    }

    public override void OnConnectedToMaster()
    {
          SceneManager.LoadScene("Lobby");
    }
}
