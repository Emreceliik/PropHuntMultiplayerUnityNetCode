using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    LobbyManager manager;

    void Start()
    {
       manager = FindAnyObjectByType<LobbyManager>();
    }
        
    
    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }
    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
   
  
}
