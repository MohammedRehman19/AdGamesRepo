using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;
    public BoardPlayer BP;
    public int ownerPlayerID;
    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];
    public List<Sprite> spriteSelection = new List<Sprite>();
    public List<Image> ImagePlayer = new List<Image>();
    public List<Image> TurnImage = new List<Image>();
    public List<RectTransform> ImagePos = new List<RectTransform>();
    public List<Toggle> AvaliblePlayer = new List<Toggle>();
    //current turn
    public string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;
    public GameObject SideImagePrefab;



    public Text currentTurnTxt;
    public int Id = 1;
 
    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you
    public void Start()
    {
     //   startColor = choosebt[0].GetComponent<Image>().color;
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight",1,7),
            Create("black_bishop",2,7), Create("black_queen",3,7), Create("black_king",4,7),
            Create("black_bishop",5,7), Create("black_knight",6,7), Create("black_rook",7,7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }

        //TurnImage[0].gameObject.SetActive(true);
        //TurnImage[1].gameObject.SetActive(false);
        //AvaliblePlayer[0].isOn = true;
        //AvaliblePlayer[1].isOn = false;
        CreateImage();
        CreateImage();
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        PhotonView photonView = obj.GetComponent<PhotonView>();
        photonView.ViewID = Id;
        Id += 1;
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void CreateImage()
    {
        GameObject obj = Instantiate(SideImagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PhotonView photonView = obj.GetComponent<PhotonView>();
        photonView.ViewID = Id;
        Id += 50005;
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localPosition = ImagePos[ImagePlayer.Count].GetComponent<RectTransform>().localPosition;
        obj.transform.localScale = Vector3.one;
        ImagePlayer.Add(obj.GetComponent<Image>());
        TurnImage.Add(obj.transform.GetChild(0).GetComponent<Image>());
        AvaliblePlayer.Add(obj.transform.GetChild(1).GetComponent<Toggle>());
       
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    
    public bool IsGameOver()
    {
        return gameOver;
    }


    public void PlayersTurn()
    {
        switch (currentPlayer)
        {
            case "white":
                currentPlayer = "black";
                break;
            case "black":
                currentPlayer = "white";
                break;
            default:
                break;
        }
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
          
            TurnImage[1].gameObject.SetActive(true);
            TurnImage[0].gameObject.SetActive(false);
            AvaliblePlayer[0].isOn = false;
            AvaliblePlayer[1].isOn = true;
        
        }
        else
        {
        
            TurnImage[1].gameObject.SetActive(false);
            TurnImage[0].gameObject.SetActive(true);
           
            AvaliblePlayer[0].isOn = true;
            AvaliblePlayer[1].isOn = false;
           
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
        if (BP == null)
            return;

        if (currentPlayer != BP.myplayer)
        {
            currentTurnTxt.text = "Wait for your Turn";

        }
        else
        {
            currentTurnTxt.text = "Your Turn";
        }

       

    }
    
    public void Winner(string playerWinner)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

   public void AvaliabliabltyChecker(int index, bool statics)
    {
     //   currentPlayer =name;   
     AvaliblePlayer[index].GetComponent<Toggle>().isOn = statics; 
    }

    public void btController(int index)
    {
        //for(int i = 0; i < choosebt.Count; i++)
        //{
        //    choosebt[i].GetComponent<Image>().color = startColor;
        //}

        //choosebt[index].GetComponent<Image>().color = new Color(1,1,1,1);
     //   startBT.gameObject.SetActive(true);
        ImagePlayer[0].sprite = spriteSelection[index];
        
        if(index == 0)
        {
            ImagePlayer[1].sprite = spriteSelection[1];
        }
        else
        {
            ImagePlayer[1].sprite = spriteSelection[0];
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
