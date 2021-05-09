using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardPlayer : MonoBehaviourPun
{

    public string myplayer = "white";
    Game main_Controller;
    PhotonView pv;


    void Start()
    {
        pv = this.GetComponent<PhotonView>();

        if (!pv.IsMine)
            return;
    
             main_Controller = GameObject.FindObjectOfType<Game>();
             main_Controller.BP = this.GetComponent<BoardPlayer>();     
    }
   
    [PunRPC]
     void Move(string refere,bool attack,int MatrixX,int MatrixY )
    {

        Chessman[] temp = GameObject.FindObjectsOfType<Chessman>();
        foreach (Chessman man in temp)
        {
            if(man.gameObject.GetComponent<PhotonView>().ViewID.ToString() == refere)
            {
                
                man.MoveObj(attack, MatrixX, MatrixY);
                
            }
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().PlayersTurn();
    }
    [PunRPC]
      void AssignImage(int Index)
      {

        StartCoroutine(waitforImage(Index));
    
    }

    private IEnumerator waitforImage(int Index)
    {
        Game main_Controller = GameObject.FindObjectOfType<Game>();
        while (main_Controller.ImagePlayer.Count < 2)
        {

            yield return new WaitForSeconds(0.5f);
        }
        main_Controller.ImagePlayer[0].sprite = main_Controller.spriteSelection[Index];
        main_Controller.ImagePlayer[1].sprite = main_Controller.spriteSelection[Index == 0 ? 1 : 0];
    }



    [PunRPC]
    void startGame()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().enabled = true;
        GameObject.Find("waitingtxt").SetActive(false);
        callAssignImage(PlayerPrefs.GetInt("ownplayer", 0));
    }
   
    public void callMove(string refere, bool attack, int MatrixX, int MatrixY)
    {    
       pv.RPC("Move", RpcTarget.All,refere,attack,MatrixX,MatrixY);
    }

    public void callAssignImage(int enterIndex)
    {
      
        pv.RPC("AssignImage", RpcTarget.All,enterIndex);
    }
    public void callstartGame()
    {
        pv.RPC("startGame", RpcTarget.All);
    }
    
}
