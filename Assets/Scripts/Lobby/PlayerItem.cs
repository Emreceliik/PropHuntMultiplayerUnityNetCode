using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    Image backgroundImage;
    public Color highlitedColor;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;
    Player player;
    private void Awake(){
        backgroundImage = GetComponent<Image>();
        playerProperties["name"] = "Emre" ;
        playerProperties["age"] = 21;
        print(playerProperties["name"]);



    }
void Start()
    {
       PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    // Update is called once per frame
    public void ApplyLocalChanges(){
        backgroundImage.color = highlitedColor;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if((int)playerProperties["playerAvatar"]== 0)
        {
        playerProperties["playerAvatar"] = avatars.Length -1;
        }
        else{
        playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"]- 1;
            
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

    }
     public void OnClickRightArrow()
    {
        if((int)playerProperties["playerAvatar"]== avatars.Length -1)
        {
        playerProperties["playerAvatar"] = 0 ;
        }
        else{
        playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
            
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if(player.CustomProperties.ContainsKey("playerAvatar")){
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }else{
            playerProperties["playerAvatar"] = 0;
        }
    }
}
