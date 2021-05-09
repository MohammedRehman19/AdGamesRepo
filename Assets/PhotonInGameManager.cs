using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
public class PhotonInGameManager : MonoBehaviourPunCallbacks
{


    void Start()
    {
        if (!PhotonNetwork.IsConnected) // 1
        {
            SceneManager.LoadScene("Launcher");
            return;
        }

        
               GameObject player1 = PhotonNetwork.Instantiate("Car",
                  Vector3.zero,
                   Quaternion.identity);
            
        
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().BP.callstartGame();
    }
    public override void OnPlayerLeftRoom(Player other)
    {
      GameObject.FindObjectOfType<Game>().AvaliabliabltyChecker(GameObject.FindObjectOfType<BoardPlayer>().myplayer == "white" ? 1 : 0,false);
    }

        void LoadArena()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        //}

        //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        //PhotonNetwork.LoadLevel("Game");

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
