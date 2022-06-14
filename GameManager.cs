using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    private int[] xLogs;
    private bool direct;
    private PowerUp power = new PowerUp();
    public Logs mLog;
    public Bird mBird;
    public Player mPlayer;
    public PlayerManager mPlayerManager;
    int xValue = 15;
    int yValue = 20;
    private int length;
    private double timeDelayB = 0.5;
    private bool direction;
    private int randomSeed;
    private bool gameStart = true;
    public List<Logs> mLogArray;
    public Queue<string> rows = new Queue<string>();
    public List<Bird> mBirdArray;
    private List<int> mTreeArray;
    public List<Car> mCarArray;
    public List<Road> mRoadArray;
    public List<Gun> mGunArray;
    private int gunCount = 0;
    private int slowDelay = 1;
    [SerializeField] private GameObject nameTxt;
    [SerializeField] private GameObject heroTxt;
    [SerializeField] private GameObject heroImage;
    [SerializeField] private GameObject profile1;
    [SerializeField] private GameObject profile2;
    [SerializeField] private GameObject profile3;
    [SerializeField] private GameObject profile4;
    public string profileName;
    [SerializeField] private Sprite human;
    [SerializeField] private Sprite mole;
    [SerializeField] private Sprite treeEnt;
    [SerializeField] private GameObject mImage;
    [SerializeField] private Sprite mBirdImage;
    [SerializeField] private Sprite mCar;
    [SerializeField] private Sprite mDrown;
    [SerializeField] private Sprite mGun;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject deadPanel;

    void Start()
    {
        profileName = Application.persistentDataPath + "/Profiles.txt";
        randomSeed = Random.Range(0, 10000);
        StartGame();
        StartBuild();
    }

    public void StartGame()
    {
        mTreeArray = new List<int>();
        mTreeArray = mBoard.Create(randomSeed);
        mBird = new Bird(randomSeed, mTreeArray);
        mBirdArray = mBird.CreateBirds(xValue, yValue);
        mBirdArray = mBird.CreateBirds(xValue, yValue);
    }

    public void Profiles()
    {
        string nameIn;
        string profile;
        string car;
        string gun;
        string drown;
        string bird;
        using (var sr = new StreamReader(profileName))
        {
            nameIn = sr.ReadLine();
            profile = sr.ReadLine();
            bird = sr.ReadLine();
            car = sr.ReadLine();
            drown = sr.ReadLine();
            gun = sr.ReadLine();
            nameTxt.GetComponent<Text>().text = "Name: " + nameIn;
            heroTxt.GetComponent<Text>().text = "Hero: " + profile;
            profile1.GetComponent<Text>().text = "Bird: " + bird;
            profile2.GetComponent<Text>().text = "Car: " + car;
            profile3.GetComponent<Text>().text = "Drown: " + drown;
            profile4.GetComponent<Text>().text = "Gun: " + gun;
            if (profile == "Human")
            {
                heroImage.GetComponent<Image>().sprite = human;
            }
            else if (profile == "Mole")
            {
                heroImage.GetComponent<Image>().sprite = mole;
            }
            else if (profile == "Treant")
            {
                heroImage.GetComponent<Image>().sprite = treeEnt;
            }
        }
    }

    public void StartBuild()
    {
        mLog = new Logs();
        mLogArray = new List<Logs>();
        mLog.InitValues(randomSeed, xValue, yValue, gameStart, mLogArray);
        mLogArray = mLog.CreateLogs(mBoard, mLogArray, yValue, xValue);
        AssetL(mLogArray, mBoard);
        mRoadArray = mBoard.GetRoads();
        mGunArray = mBoard.GetGuns();
        rows = mBoard.GetRows();
        Debug.Log("Start");
        mPlayer = new Player();
        mPlayer = mPlayerManager.Setup(mBoard, mPlayer, xValue, yValue, this);
        mPlayer.SetAlive(true);
    }

    void Update()
    {
        if (!gameStart)
        {
            if (mPlayer.GetAlive())
            {
                AssetC(mBoard, mRoadArray);
                AssetB(mBoard, mBirdArray, mTreeArray);
                AssetL(mLogArray, mBoard);
            }
        }
    }

    public void begin()
    {
        gameStart = false;
    }

    void FixedUpdate()
    {
        if (!gameStart)
        {
            power = mPlayerManager.ReturnPU();
            if (power.Slow() != -1)
            {
                slowDelay = 2;
            }
            else
            {
                slowDelay = 1;
            }

            if (power.Freeze() == -1)
            {
                if (Time.time >= timeDelayB)
                {
                    if (mPlayer.GetAlive())
                    {
                        timeDelayB = Mathf.FloorToInt(Time.time) + slowDelay;
                        BirdDelay(mBoard);
                        CarDelay(mBoard, mRoadArray);
                        LogDelay(mBoard);
                        GunDelay(mBoard);
                    }
                }
            }
        }
    }

    void GunDelay(Board mBoard)
    {
        Cell[,] mAllAssets = mBoard.GetAllAssets();
        gunCount++;
        int x;
        mBoard.DestroyFire();
        for (int i = 0; i < mGunArray.Count; i++)
        {
            if (mGunArray[i].Shoot(gunCount))
            {
                int y = mGunArray[i].GetY();
                if (mPlayer.GetMapY() == y)
                {
                    Debug.Log("Gun Death");
                    if (power.GetAngel())
                    {
                        Debug.Log("Angel");
                        power.Angel(mPlayer, rows);
                    }
                    else
                    {
                        mPlayer.SetAlive(false);
                        Observer observer = new Observer();
                        observer.SetObs(new DeathScenes());
                        observer.Notify("Gun", mPlayerManager, mImage, mBirdImage, mCar, mDrown, mGun, deadPanel,
                            canvas, this);
                        // mPlayer.Death("Gun");
                        canvas.SetActive(false);
                    }
                }

                mBoard.gunFired(i);
            }
        }
    }

    void CarDelay(Board mBoard, List<Road> mRoadArray)
    {
        int vol;
        List<Car> checkCars;
        int rowAdd = 0;
        for (int j = 0; j < rows.Count; j++)
        {
            if (rows.ToArray()[j] != "Road")
            {
                rowAdd++;
            }
            else
            {
                j = rows.Count;
            }
        }

        for (int i = 0; i < mRoadArray.Count; i++)
        {
            vol = mRoadArray[i].GetVolume();
            checkCars = mRoadArray[i].CheckCars(xValue, vol);
            mRoadArray[i].MoveCars(checkCars);
            mCarArray = mRoadArray[i].GetCars();
            for (int j = 0; j < mCarArray.Count; j++)
            {
                if (mPlayer.GetX() == mCarArray[j].GetX() && mPlayer.GetMapY() == rowAdd + i &&
                    rows.ToArray()[mPlayer.GetMapY()] == "Road")
                {
                    Debug.Log("Car Death");
                    if (power.GetAngel())
                    {
                        Debug.Log("Angel");
                        power.Angel(mPlayer, rows);
                    }
                    else
                    {
                        mPlayer.SetAlive(false);
                        Observer observer = new Observer();
                        observer.SetObs(new DeathScenes());
                        observer.Notify("Car", mPlayerManager, mImage, mBirdImage, mCar, mDrown, mGun, deadPanel,
                            canvas, this);
                        canvas.SetActive(false);
                    }
                }
            }
        }

        AssetC(mBoard, mRoadArray);
    }

    void BirdDelay(Board mBoard)
    {
        mBirdArray = mBird.GetBirdArray();
        mTreeArray = mBird.GetTreeArray();
        for (int i = 0; i < mBirdArray.Count; i++)
        {
            mBirdArray[i].MoveBird(mTreeArray);
            if (mPlayer.GetX() == mBirdArray[i].GetX() && mPlayer.GetMapY() == mBirdArray[i].GetY())
            {
                Debug.Log("Bird Death");
                if (power.GetAngel())
                {
                    Debug.Log("Angel");
                    power.Angel(mPlayer, rows);
                }
                else
                {
                    mPlayer.SetAlive(false);
                    Observer observer = new Observer();
                    observer.SetObs(new DeathScenes());
                    observer.Notify("Bird", mPlayerManager, mImage, mBirdImage, mCar, mDrown, mGun, deadPanel, canvas,
                        this);
                    canvas.SetActive(false);
                }
            }
        }

        AssetB(mBoard, mBirdArray, mTreeArray);
    }

    void LogDelay(Board mBoard)
    {
        bool found = false;
        bool delete = false;
        Queue<string> logIndex = new Queue<string>();
        int rowAdd = 0;
        for (int j = 0; j < rows.Count; j++)
        {
            if (rows.ToArray()[j] != "River")
            {
                rowAdd++;
            }
            else
            {
                j = rows.Count;
            }
        }

        for (int i = 0; i < mLogArray.Count; i++)
        {
            int[] xLog = mLogArray[i].GetXCoord();
            rows = mBoard.GetRows();
            int x = mPlayer.GetX();
            int y = mPlayer.GetMapY();
            if (rows.ToArray()[y] == "River")
            {
                for (int j = 0; j < xLog.Length; j++)
                {
                    if (xLog[j] == x && (mLogArray[i].GetRow() + rowAdd) == y)
                    {
                        if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||
                              Input.GetKey(KeyCode.DownArrow) ||
                              Input.GetKey(KeyCode.S)))
                        {
                            if (mLogArray[i].GetDirection())
                            {
                                x++;
                            }
                            else
                            {
                                x--;
                            }
                        }

                        mPlayer.SetX(x);
                        Player[,] mPlayers = mPlayerManager.GetPlayers();
                        List<GameObject> newPlayers = mPlayerManager.GetNewPlayers();
                        GameObject newPlayer = null;
                        newPlayer = Instantiate(mPlayerManager.GetPrefab(), transform);
                        newPlayers.Insert(0, newPlayer);
                        RectTransform rectTransformA = newPlayers[0].GetComponent<RectTransform>();
                        rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                        mPlayers[x, y] = newPlayers[0].GetComponent<Player>();
                        mPlayers[x, y].Setup(new Vector2Int(x, y), mBoard, mPlayerManager);
                        mPlayers[x, y].GetComponent<Image>().sprite = mPlayerManager.GetPic();
                        if (newPlayers.Count < 1)
                        {
                            Destroy(newPlayers[1]);
                        }

                        found = true;
                    }
                }
            }

            if (!found)
            {
                if (mPlayer.GetAlive())
                {
                    if (mBoard.CheckRiver(mPlayer))
                    {
                        PowerUp power = mBoard.GetPower();
                        if (power.GetAngel())
                        {
                            power.Angel(mPlayer, mBoard.GetRows());
                        }
                        else
                        {
                            power.SetFreeze();
                            mPlayer.SetAlive(false);
                            Observer observer = new Observer();
                            observer.SetObs(new DeathScenes());
                            observer.Notify("Drown", mPlayerManager, mImage, mBirdImage, mCar, mDrown, mGun, deadPanel,
                                canvas, this);
                            canvas.SetActive(false);
                        }
                    }
                }
            }

            if (!mLogArray[i].MoveLog(mLogArray[i].GetXCoord(), mLogArray[i].GetDirection()))
            {
                delete = true;
            }


            if (mLogArray[i].GetXCoord()[0] < -4 || mLogArray[i].GetXCoord()[0] > xValue + 4)
            {
                delete = true;
            }

            if (delete)
            {
                mLogArray.RemoveAt(i);
                i--;
                delete = false;
            }
        }

        if (mLogArray.Count != 0)
        {
            mLogArray[0].CheckCollision();
            mLogArray[0].CreateLogs(mBoard, mLogArray, yValue, xValue);
        }


        AssetL(mLogArray, mBoard);
    }

    void AssetB(Board mBoard, List<Bird> birds, List<int> tree)
    {
        mBoard.GenerateBirds(xValue, yValue, birds, tree);
    }

    void AssetC(Board mBoard, List<Road> road)
    {
        mBoard.GenerateCar(xValue, yValue, road);
    }

    void AssetL(List<Logs> mLogArrayAsset, Board mBoard)
    {
        mBoard.GenerateLogs(xValue, yValue, mLogArrayAsset, mBoard);
    }
}