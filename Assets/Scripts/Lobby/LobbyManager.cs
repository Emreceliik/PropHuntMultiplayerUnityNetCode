using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    
    public InputField roomNameInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text roomName;
    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contenObject;
    public float timeBetweenUpdates = 1.5f;
    float nextUpdate;
    List<PlayerItem> playerItemList = new List<PlayerItem>();
    public PlayerItem playerItemprefab;
    public Transform playerItemParent;
    public GameObject playButton;

    void Start()
    {
        PhotonNetwork.JoinLobby();
        
    }

    public void OnClickCreate()
    {
        if(roomNameInput.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions(){MaxPlayers = 5 , BroadcastPropsChangeToAll =true});
         

        }
        
    }
    // Update is called once per frame
     public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text =  "Room Name: "+PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= nextUpdate)
        {
            UpdateRoomList(roomList); 
            nextUpdate = Time.time + timeBetweenUpdates;
        }
       

    }
   void UpdateRoomList(List<RoomInfo> List)
    {
        foreach(RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();
            foreach(RoomInfo room in List)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab,contenObject)  ;
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom) ;
        }
    }
     public void JoinRoom(string roomName)
     {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
       lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }
   public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    void UpdatePlayerList(){
        foreach(PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();
        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int , Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemprefab,playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }
            playerItemList.Add(newPlayerItem);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        
    }
     public override void OnPlayerLeftRoom(Player newPlayer)
    {
        UpdatePlayerList();
        
    }
    void Update(){
        if(PhotonNetwork.IsMasterClient &&PhotonNetwork.CurrentRoom.PlayerCount >= 1){
            playButton.SetActive(true);
        }
        else{
            playButton.SetActive(false);
        }
    }
    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("Game");


    }

}
    
