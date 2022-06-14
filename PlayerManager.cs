using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Sprite humanN;
    [SerializeField] private Sprite moleN;
    [SerializeField] private GameObject mImage;
    [SerializeField] private Sprite treantN;
    [SerializeField] private Sprite humanE;
    [SerializeField] private Sprite moleE;
    [SerializeField] private Sprite treantE;
    [SerializeField] private Sprite humanS;
    [SerializeField] private Sprite moleS;
    [SerializeField] private Sprite treantS;
    [SerializeField] private Sprite humanW;
    [SerializeField] private Sprite moleW;
    [SerializeField] private Sprite treantW;
    [SerializeField] private GameObject deadImage;
    [SerializeField] private Sprite mDrown;
    [SerializeField] private Sprite mCar;
    [SerializeField] private Sprite mBird;
    [SerializeField] private Sprite mGun;
    public GameObject canvas;
    public GameObject deadPanel;
    private double timeDelayP = 2;
    public Player[,] mPlayers;
    public GameObject mPlayerPrefab;
    public GameObject newPlayer = null;
    public List<GameObject> newPlayers;
    public Sprite mPlayer;
    private string name;
    private int xValue;
    private int yValue;
    private int mapHeight = 0;
    private int totalHeight = 0;
    private int currentHeight = 0;
    public Player player;
    public Board mBoard;
    public int lastInp = 5;
    private PowerUp power;
    private GameManager gameManager;
    private bool blinkPress = false;
    public string leaderboard;
    public int count = 0;

    void Start()
    {
        leaderboard = Application.persistentDataPath + "/leaderboard.txt";
    }

	public GameManager GetGameManager(){
		return gameManager;
	}

    public Player Setup(Board board, Player player1, int xValues, int yValues, GameManager gm)
    {
        mBoard = board;
        xValue = xValues;
        yValue = yValues;
        player = player1;
        player = PlacePlayer(mBoard, player1, xValue, yValue);
        mPlayers = new Player[xValue, yValue];
        newPlayer = Instantiate(mPlayerPrefab, transform);
        gameManager = gm;
        return player;
    }

    public Sprite GetHN()
    {
        return humanN;
    }

    public Sprite GetHE()
    {
        return humanE;
    }

    public Sprite GetHS()
    {
        return humanS;
    }

    public Sprite GetHW()
    {
        return humanW;
    }

    public Sprite GetMN()
    {
        return moleN;
    }

    public Sprite GetME()
    {
        return moleE;
    }

    public Sprite GetMS()
    {
        return moleS;
    }

    public Sprite GetMW()
    {
        return moleW;
    }

    public Sprite GetTN()
    {
        return treantN;
    }

    public Sprite GetTE()
    {
        return treantE;
    }

    public Sprite GetTS()
    {
        return treantS;
    }

    public Sprite GetTW()
    {
        return treantW;
    }

    public PowerUp ReturnPU()
    {
        return power;
    }

    public void SetName(string name1)
    {
        name = name1;
    }

    public string GetName()
    {
        return name;
    }

    private Player PlacePlayer(Board board, Player player1, int xValue, int yValue)
    {
        return player1.GeneratePlayer(board, xValue, yValue, mPlayer);
    }

    public List<GameObject> GetNewPlayers()
    {
        return newPlayers;
    }

    public GameObject GetPrefab()
    {
        return mPlayerPrefab;
    }

    public Sprite GetPic()
    {
        return mPlayer;
    }

    public Player[,] GetPlayers()
    {
        return mPlayers;
    }

    public int GetTotalHeight()
    {
        return totalHeight;
    }

    public int GetMapHeight()
    {
        return mapHeight;
    }

    void FixedUpdate()
    {
        if (player.GetAlive())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                blinkPress = true;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                lastInp = 2;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                lastInp = 3;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                lastInp = 0;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                lastInp = 1;
            }

            if (Time.time % 1 == 0)
            {
                power = mBoard.GetPower();
                if (blinkPress && power.GetBlink())
                {
                    power.Blink(player);
                    blinkPress = false;
                }
                else
                {
                    if (lastInp == 2)
                    {
                        player.Move(2, mBoard, this);
                        powerMove();
                    }

                    if (lastInp == 3)
                    {
                        player.Move(3, mBoard, this);
                        powerMove();
                    }

                    if (lastInp == 0)
                    {
                        player.Move(0, mBoard, this);
                        powerMove();
                    }

                    if (lastInp == 1)
                    {
                        player.Move(1, mBoard, this);
                        powerMove();
                    }
                }
            }

            AddAsset();
        }
        else
        {
            if (count == 0)
            {
                StreamWriter file = File.AppendText(leaderboard);
                file.WriteLine(name);
                file.WriteLine(totalHeight);
                file.Close();
                count++;
                Observer observer = new Observer();
                observer.SetObs(new DeathScenes());
                observer.Notify("Drown", this, mImage, mBird, mCar, mDrown, mGun, deadPanel,
                    canvas, GetGameManager());
                canvas.SetActive(false);
            }
        }
    }

    public void powerMove()
    {
        if (power.GetFreeze())
        {
            power.FreezeUp();
        }

        if (power.GetSlow())
        {
            power.SlowUp();
        }

        blinkPress = false;
    }

    public int GetLast()
    {
        return lastInp;
    }

    public void AddAsset()
    {
        switch (player.GetImages(lastInp,this))
        {
            case 0:
                mPlayer = moleW;
                break;
            case 1:
                mPlayer = moleE;
                break;
            case 2:
                mPlayer = moleN;
                break;
            case 3:
                mPlayer = moleS;
                break;
            case 4:
                mPlayer = treantW;
                break;
            case 5:
                mPlayer = treantN;
                break;
            case 6:
                mPlayer = treantE;
                break;
            case 7:
                mPlayer = treantS;
                break;
            case 8:
                mPlayer = humanW;
                break;
            case 9:
                mPlayer = humanN;
                break;
            case 10:
                mPlayer = humanE;
                break;
            case 11:
                mPlayer = humanS;
                break;
        }

        int x = 0;
        int y = 0;
        currentHeight = player.GetY();
        if (player.GetY() > totalHeight)
        {
            totalHeight = player.GetY();
            mapHeight++;
        }

        if (mapHeight > 3)
        {
            mBoard.MoveUp();
            mapHeight = 3;
            mBoard.PowerUpCheck(player.GetX(), mapHeight);
        }

        if (totalHeight != currentHeight)
        {
            y = currentHeight + (mapHeight - currentHeight) - (totalHeight - currentHeight);
        }
        else if (totalHeight > 3)
        {
            y = currentHeight + (mapHeight - currentHeight);
        }
        else
        {
            y = currentHeight;
        }

        x = player.GetX();
        newPlayers.Add(newPlayer);
        RectTransform rectTransformA = newPlayers[newPlayers.Count - 1].GetComponent<RectTransform>();
        rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
        mPlayers[x, y] = newPlayers[newPlayers.Count - 1].GetComponent<Player>();
        mPlayers[x, y].Setup(new Vector2Int(x, y), mBoard, this);
        mPlayers[x, y].GetComponent<Image>().sprite = mPlayer;
        if (newPlayers.Count < 1)
        {
            Destroy(newPlayers[0]);
        }

        player.SetMapY(y);
        lastInp = 5;
    }
}