using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class ShowPlayerNicknames : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = photonView.Owner.NickName.Split('-')[0];
    }


    // Update is called once per frame
    void Update()
    {
        Camera camera = (Camera)FindObjectOfType(typeof(Camera));
        if (camera)
        {
            Debug.Log("Found!");
            transform.LookAt(camera.gameObject.transform);
        }
    }
}
