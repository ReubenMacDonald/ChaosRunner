using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Image mOutlineImage;
    public RectTransform mRectTransform = null;
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Board mBoard = new Board();
    public int x;
    private string death = "";
    public int y;
    public int mapY;
    private List<Logs> mLogArray;
    public int xValue;
    public int yValue;
    protected PlayerManager mPlayerManager;
    public Animator mAnim = null;
    public Sprite mPlayer;
    private bool alive = false;
    [SerializeField] private GameObject mImage;
    [SerializeField] private Sprite mBird;
    [SerializeField] private Sprite mCar;
    [SerializeField] private Sprite mDrown;
    [SerializeField] private Sprite mGun;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject deadPanel;

    public virtual void Setup(Vector2Int newBoardPosition, Board board, PlayerManager playerManager)
    {
        mBoardPosition = newBoardPosition;
        mPlayerManager = playerManager;
        mRectTransform = GetComponent<RectTransform>();
        mBoard = board;
    }

    public Player GeneratePlayer(Board board, int xValue1, int yValue1, Sprite mPlayer1)
    {
        y = 0;
        xValue = xValue1;
        yValue = yValue1;
        x = xValue / 2;
        mPlayer = mPlayer1;
        return this;
    }

    public int GetImages(int direct, PlayerManager mPM)
    {
        Images images = new Images(mPM.GetName());
        string imageName = images.GetSprite(direct);
        switch (imageName)
        {
            case "MoleW":
                return 0;
            case "MoleE":
                return 1;
            case "MoleN":
                return 2;
            case "MoleS":
                return 3;
            case "TreantW":
                return 4;
            case "TreantN":
                return 5;
            case "TreantE":
                return 6;
            case "TreantS":
                return 7;
            case "HumanW":
                return 8;
            case "HumanN":
                return 9;
            case "HumanE":
                return 10;
            case "HumanS":
                return 11;
            case "Human":
                return 11;
            case "Mole":
                return 3;
            case "Treant":
                return 7;
        }

        return -1;
    }

    public void SetMapY(int mY)
    {
        mapY = mY;
    }

    public int GetMapY()
    {
        return mapY;
    }

    public void SetAlive(bool alive1)
    {
        alive = alive1;
    }

    public bool GetAlive()
    {
        return alive;
    }

    public void MoveAvailiable(int direction, Board board, PlayerManager mPM, bool log)
    {
        List<int> trees;
        List<Gun> gunsArray;
        bool fail = false;
        if (x < 1)
        {
            x = 1;
        }

        if (x > 13)
        {
            x = 13;
        }

        trees = board.GetTrees();
        for (int i = 0; i < trees.Count / 2; i++)
        {
            if (trees.ToArray()[(i * 2)] == x && trees.ToArray()[(i * 2) + 1] == mapY)
            {
                Debug.Log("Fail");
                fail = true;
            }
        }

        gunsArray = board.GetGuns();
        for (int i = 0; i < gunsArray.Count; i++)
        {
            if (direction == 1 || direction == 0)
            {
                if (gunsArray[i].GetY() == mapY)
                {
                    if (gunsArray[i].GetDirect() && x == 0)
                    {
                        Debug.Log("Fail1");
                        fail = true;
                    }
                    else if (!gunsArray[i].GetDirect() && x == 14)
                    {
                        Debug.Log("Fail2");
                        fail = true;
                    }
                }
            }
            else
            {
                if (gunsArray[i].GetY() == mapY + 1)
                {
                    Debug.Log("!");
                    if (gunsArray[i].GetDirect() && x == 0)
                    {
                        Debug.Log("Fail1");
                        fail = true;
                    }
                    else if (!gunsArray[i].GetDirect() && x == 14)
                    {
                        Debug.Log("Fail2");
                        fail = true;
                    }
                }
            }
        }

        if (fail)
        {
            switch (direction)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    x--;
                    break;
                case 2:
                    y--;
                    break;
                case 3:
                    y++;
                    break;
            }

            SetY(y);
            SetX(x);
        }
    }

    public void Move(int direct1, Board board, PlayerManager mPM)
    {
        List<int> trees;
        List<Gun> gunsArray;
        int direct = direct1;
        mBoard = board;
        mLogArray = board.GetmLogArray();
        int[] xLog;
        bool log = false;
        int logI = 0;
        bool fail = false;
        for (int i = 0; i < mLogArray.Count; i++)
        {
            xLog = mLogArray[i].GetXCoord();
            for (int j = 0; j < xLog.Length; j++)
            {
                if (xLog[j] == x && mLogArray[i].GetRow() == y)
                {
                    logI = i;
                    log = true;
                }
            }
        }

        trees = board.GetTrees();
        for (int i = 0; i < trees.Count / 2; i++)
        {
            int mapYTemp = mapY;
            if (direct == 2)
            {
                if (trees.ToArray()[(i * 2)] == x && trees.ToArray()[(i * 2) + 1] == mapYTemp + 1)
                {
                    Debug.Log("Fail");
                    fail = true;
                }
            }

            if (direct == 3)
            {
                if (trees.ToArray()[(i * 2)] == x && trees.ToArray()[(i * 2) + 1] == mapYTemp - 1)
                {
                    Debug.Log("Fail");
                    fail = true;
                }
            }
        }

        if (!fail)
        {
            if (direct == 2)
            {
                y++;
            }

            if (direct == 3)
            {
                if (y != (mPM.GetTotalHeight() - 3))
                {
                    y--;
                }
            }
        }

        if (direct == 0)
        {
            if (x != 0)
            {
                if (log)
                {
                    if (mLogArray[logI].GetDirection())
                    {
                        x--;
                    }
                }
                else
                {
                    x--;
                }
            }
        }

        if (direct == 1)
        {
            if (x != 0)
            {
                if (log)
                {
                    if (!mLogArray[logI].GetDirection())
                    {
                        x++;
                    }
                }
                else
                {
                    x++;
                }
            }
        }

        MoveAvailiable(direct, board, mPM, log);
        if (mBoard.CheckRiver(this))
        {
            PowerUp power = mBoard.GetPower();
            if (power.GetAngel())
            {
                power.Angel(this, mBoard.GetRows());
            }
            else
            {
                power.SetFreeze();
                SetAlive(false);
                Observer observer = new Observer();
                observer.SetObs(new DeathScenes());
                observer.Notify("Drown", mPM, mImage, mBird, mCar, mDrown, mGun, deadPanel, canvas,
                    mPM.GetGameManager());
                canvas.SetActive(false);
            }
        }
    }

    public int GetX()
    {
        return x;
    }

    public void SetX(int x1)
    {
        x = x1;
        if (x < 1)
        {
            x = 1;
        }

        if (x > 13)
        {
            x = 13;
        }
    }

    public int GetY()
    {
        return y;
    }

    public void SetY(int y1)
    {
        y = y1;
    }

    public void SetYAngel(int y1)
    {
        for (int i = 0; i < y1; i++)
        {
            mBoard.MoveUp();
        }

        y = y + y1;
    }
}