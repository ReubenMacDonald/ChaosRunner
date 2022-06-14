using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;
    public GameObject mPlayerPrefab;
    public Sprite mGunLeft;
    public Sprite mGunRight;
    public Sprite mTreeSprite;
    public Sprite mLogSprite;
    public Sprite mCarRSprite;
    public Sprite mCarLSprite;
    public Sprite mAngelSprite;
    public Sprite mSlowSprite;
    public Sprite mFreezeSprite;
    public Sprite mBlinkSprite;
    public Sprite sprite;
    private int total = 0;
    public GameObject newPlayer = null;
    public List<GameObject> newPlayers;
    public List<GameObject> cellsList;
    public List<GameObject> birdsList;
    public List<GameObject> carsList;
    public List<GameObject> logsList;
    public List<GameObject> gunList;
    public List<GameObject> treesList;
    public List<GameObject> powerList;
    public Cell[,] mAllCells;
    public Cell[,] mAllAssets = new Cell[15, 20];
    public Player[,] mPlayers;
    private int xValue = 15;
    private int yValue = 20;
    public int keepCell = 0;
    public SpriteRenderer _renderer;
    public List<Bird> mBirdArray = new List<Bird>();
    public List<Gun> mGunArray = new List<Gun>();
    private List<int> treeArray = new List<int>();
    private List<Logs> mLogArray = new List<Logs>();
    public GameObject newCar;
    public List<Road> mRoadArray = new List<Road>();
    private int fieldCount = 0;
    private bool powerUpOnBoard = false;
    public PowerUp powerUp;
    [SerializeField] private Sprite birdImageR;
    [SerializeField] private Sprite birdImageL;
    public List<GameObject> gunFire = new List<GameObject>();
    public Sprite mFireSprite;

    enum maps
    {
        Forest,
        Field,
        River,
        Road
    };

    public Queue<string> rows;

    /*
     * 4 "maps"
     *
     * 1. Forest
     * 2. Field
     * 3. River
     * 4. Road
    */

    public List<int> Create(int randomSeed)
    {
        rows = new Queue<string>();
        Random.InitState(randomSeed);
        mAllCells = new Cell[xValue, yValue];
        mRoadArray = new List<Road>();
        mPlayers = new Player[xValue, yValue];
        int rand;
        for (int y = 0; y < yValue; y++)
        {
            for (int x = 0; x < xValue; x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);
                cellsList.Add(newCell);
                keepCell++;
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        AddPlain();
        AddPlain();
        total = 4;
        for (int i = 0; i < 4; i++)
        {
            if (total < 20)
            {
                total += 7;
                rand = Random.Range(1, 5);
                switch (rand)
                {
                    case 1:
                        AddForest();
                        break;
                    case 2:
                        AddField();
                        break;
                    case 3:
                        AddRiver();
                        break;
                    case 4:
                        AddRoad();
                        break;
                }
            }
        }
        powerUp = new PowerUp();
        return GenerateBoard(xValue, yValue); //Total = 23
    }

    public Cell[,] GetAllAssets()
    {
        return mAllAssets;
    }

    public GameObject GetCellPrefab()
    {
        return mCellPrefab;
    }

    public void AddForest()
    {
        for (int i = 0; i < 5; i++)
        {
            rows.Enqueue("Forest");
        }

        AddPlain();
    }

    public void AddField()
    {
        for (int i = 0; i < 5; i++)
        {
            rows.Enqueue("Field");
        }

        AddPlain();
    }

    public void AddRiver()
    {
        for (int i = 0; i < 5; i++)
        {
            rows.Enqueue("River");
        }

        AddPlain();
    }

    public void AddRoad()
    {
        for (int i = 0; i < 5; i++)
        {
            rows.Enqueue("Road");
            mRoadArray.Add(new Road(Random.Range(0, 2), xValue, Random.Range(3, 8)));
        }

        AddPlain();
    }

    public void AddPlain()
    {
        rows.Enqueue("Plain");
        rows.Enqueue("Plain");
    }

    public void MoveUp()
    {
        if (rows.Count == 20)
        {
            CheckTop();
        }

        if (rows.ToArray()[0] == "River")
        {
            for (int i = 0; i < mLogArray.Count; i++)
            {
                mLogArray[i].DownRow();
            }
        }
        else if (rows.ToArray()[0] == "Road")
        {
            mRoadArray.RemoveAt(0);
        }

        if (rows.ToArray()[20] == "Field")
        {
            NewField();
        }

        rows.Dequeue();
        Move();
        switch (rows.ToArray()[19])
        {
            case "Forest":
                NewForest();
                break;
            case "River":
                NewRiver();
                break;
        }
    }

    public void DestroyFire()
    {
        for (int i = 0; i < gunFire.Count; i++)
        {
            Destroy(gunFire[i]);
            gunFire.RemoveAt(i);
            i--;
        }
    }
    public void gunFired(int i)
    {
        int x;
        int y;
        y = mGunArray[i].GetY();
        GameObject newFire = Instantiate(mCellPrefab, transform);
        gunFire.Add(newFire);
        RectTransform rectTransformA = newFire.GetComponent<RectTransform>();
        if (mGunArray[i].GetDirect())
        {
            x = 1;
        }
        else
        {
            x = 13;
        }

        rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
        mAllAssets[x, y] = newFire.GetComponent<Cell>();
        mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
        mAllAssets[x, y].GetComponent<Image>().sprite = mFireSprite;
    }

    public void NewForest()
    {
        bool bird = false;
        bool row = false;
        int x = 0;
        int y = 19;
        int i = 0;
        while (rows.ToArray()[19 - i] == "Forest")
        {
            i++;
        }

        if (i != 1 && i != 5)
        {
            row = true;
        }

        while (!bird)
        {
            if (Random.Range(0, 2) == 0)
            {
                Bird b = new Bird(x, y, true, false, xValue, yValue, mBirdArray);
            }
            else
            {
                Bird b = new Bird(xValue - 1, y, true, false, xValue, yValue, mBirdArray);
            }

            treeArray.Add(x);
            treeArray.Add(y);
            GameObject newTree = Instantiate(mCellPrefab, transform);
            treesList.Add(newTree);
            RectTransform rectTransformA = newTree.GetComponent<RectTransform>();
            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
            mAllAssets[x, y] = newTree.GetComponent<Cell>();
            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
            mAllAssets[x, y].GetComponent<Image>().sprite = mTreeSprite;
            if (x == xValue - 1)
            {
                bird = true;
            }
            else
            {
                x = xValue - 1;
            }
        }

        if (row == true)
        {
            x = Random.Range(2, xValue - 2);
            treeArray.Add(x);
            treeArray.Add(y);
            GameObject newTree = Instantiate(mCellPrefab, transform);
            treesList.Add(newTree);
            RectTransform rectTransformA = newTree.GetComponent<RectTransform>();
            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
            mAllAssets[x, y] = newTree.GetComponent<Cell>();
            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
            mAllAssets[x, y].GetComponent<Image>().sprite = mTreeSprite;
        }
    }

    public void NewField()
    {
        int x = 14;
        int y = 20;
        if (Random.Range(0, 2) == 0)
        {
            mGunArray.Add(new Gun(y, false, Random.Range(1, 4)));
        }
        else
        {
            mGunArray.Add(new Gun(y, true, Random.Range(1, 4)));
        }
    }

    public void NewRiver()
    {
        Logs l = new Logs();
        mLogArray = l.CreateLogs(this, mLogArray, yValue, xValue);
    }

    public void Move()
    {
        int keepTree = 0;
        for (int p = 0; p < xValue; p++)
        {
            for (int q = 0; q < yValue; q++)
            {
                switch (rows.ToArray()[q])
                {
                    case "Forest":
                        mAllCells[p, q].GetComponent<Image>().color = new Color32(0, 100, 0, 255);
                        break;
                    case "Field":
                        fieldCount++;
                        mAllCells[p, q].GetComponent<Image>().color = new Color32(34, 139, 34, 255);
                        break;
                    case "River":
                        mAllCells[p, q].GetComponent<Image>().color = new Color32(0, 206, 209, 255);
                        if (q == 0)
                        {
                            CheckBottom("river", 0, 0);
                        }

                        break;
                    case "Road":
                        mAllCells[p, q].GetComponent<Image>().color = new Color32(105, 105, 105, 255);
                        break;
                    case "Plain":
                        if (q == 0)
                        {
                            CheckBottom("river", 0, 0);
                        }

                        mAllCells[p, q].GetComponent<Image>().color = new Color32(144, 238, 144, 255);
                        break;
                }
            }
        }

        fieldCount = fieldCount / 15;
        if (fieldCount == 0 && gunList.Count != 0)
        {
            mGunArray.RemoveAt(0);
            Destroy(gunList[0]);
            gunList.RemoveAt(0);
        }

        for (int i = 0; i < fieldCount; i++)
        {
            mGunArray[i].Down();
            int yG = mGunArray[i].GetY();
            if (yG < 0)
            {
                mGunArray.RemoveAt(i);
                Destroy(gunList[i]);
                gunList.RemoveAt(i);
                i--;
            }
            else
            {
                GameObject newGun = Instantiate(mCellPrefab, transform);
                if (gunList.Count > i)
                {
                    Destroy(gunList[i]);
                    gunList[i] = newGun;
                }
                else
                {
                    gunList.Add(newGun);
                }

                if (mGunArray[i].GetDirect())
                {
                    RectTransform rectTransformA = newGun.GetComponent<RectTransform>();
                    rectTransformA.anchoredPosition = new Vector2((0 * 100) + 50, (yG * 100) + 50);
                    mAllAssets[0, yG] = newGun.GetComponent<Cell>();
                    mAllAssets[0, yG].Setup(new Vector2Int(0, yG), this);
                    mAllAssets[0, yG].GetComponent<Image>().sprite = mGunLeft;
                }
                else if (!mGunArray[i].GetDirect())
                {
                    RectTransform rectTransformA = newGun.GetComponent<RectTransform>();
                    rectTransformA.anchoredPosition = new Vector2((14 * 100) + 50, (yG * 100) + 50);
                    mAllAssets[xValue - 1, yG] = newGun.GetComponent<Cell>();
                    mAllAssets[xValue - 1, yG].Setup(new Vector2Int(0, yG), this);
                    mAllAssets[xValue - 1, yG].GetComponent<Image>().sprite = mGunRight;
                }
            }
        }

        int bCount = mBirdArray.Count;
        for (int j = 0; j < bCount; j++)
        {
            mBirdArray[j].DecreaseY();
            if (CheckBottom("bird", j, j))
            {
                j--;
                bCount = mBirdArray.Count;
            }
        }

        int count = treeArray.Count / 2;
        int x;
        int y;
        for (int j = 0; j < count; j++)
        {
            if (CheckBottom("tree", j, (j * 2) + 1))
            {
                keepTree++;
                treeArray[(j * 2) + 1] = treeArray[(j * 2) + 1] - 1;
                x = treeArray[(j * 2)];
                y = treeArray[(j * 2) + 1];
                GameObject newTree = Instantiate(mCellPrefab, transform);
                Destroy(treesList[j]);
                treesList.RemoveAt(j);
                treesList.Insert(j, newTree);
                RectTransform rectTransformA = newTree.GetComponent<RectTransform>();
                rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                mAllAssets[x, y] = newTree.GetComponent<Cell>();
                mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                mAllAssets[x, y].GetComponent<Image>().sprite = mTreeSprite;
            }
            else
            {
                j--;
                count = treeArray.Count / 2;
            }
        }

        if (powerUp.SetY(powerUp.GetY() - 1))
        {
            powerUpOnBoard = false;
        }
    }

    public bool CheckBottom(string type, int birdNo, int treeNo)
    {
        switch (type)
        {
            case "bird":
                if (mBirdArray[birdNo].GetY() < 0)
                {
                    mBirdArray.RemoveAt(birdNo);
                    if (birdsList.Count > 0)
                    {
                        Destroy(birdsList[birdNo]);
                        birdsList.RemoveAt(birdNo);
                    }

                    return true;
                }

                return false;
            case "tree":
                if (treeArray[treeNo] == 0)
                {
                    treeArray.RemoveAt(treeNo - 1);
                    treeArray.RemoveAt(treeNo - 1);
                    Destroy(treesList[birdNo]);
                    treesList.RemoveAt(birdNo);
                    return false;
                }

                return true;
            case "river":
                for (int i = 0; i < mLogArray.Count; i++)
                {
                    if (mLogArray[i].GetRow() == -1)
                    {
                        mLogArray.RemoveAt(i);
                        Destroy(logsList[i]);
                        logsList.RemoveAt(i);
                    }
                }

                break;
        }

        return false;
    }

    public void NewChoice()
    {
        int rand = Random.Range(1, 5);
        switch (rand)
        {
            case 1:
                AddForest();
                break;
            case 2:
                AddField();
                break;
            case 3:
                AddRiver();
                break;
            case 4:
                AddRoad();
                break;
        }
    }

    public List<Road> GetRoads()
    {
        return mRoadArray;
    }

    public void CheckTop()
    {
        int length;
        int count = 1;
        string last;
        string check;
        length = rows.ToArray().Length;
        length--;
        last = rows.ToArray()[length];
        check = rows.ToArray()[length - 1];
        while (check == last)
        {
            check = rows.ToArray()[length - count];
            count++;
        }

        count--;
        if (count == 2)
        {
            NewChoice();
        }
        else
        {
            if (last.Equals(maps.Forest))
            {
                rows.Enqueue("Forest");
            }
            else if (last.Equals(maps.Field))
            {
                rows.Enqueue("Field");
            }
            else if (last.Equals(maps.River))
            {
                rows.Enqueue("River");
            }
            else if (last.Equals(maps.Road))
            {
                rows.Enqueue("Road");
                mRoadArray.Add(new Road(Random.Range(0, 2), xValue, Random.Range(3, 8)));
            }
        }
    }

    public List<int> GetTrees()
    {
        return treeArray;
    }
    
    private List<int> GenerateBoard(int xValue, int yValue)
    {
        int lr;
        bool row = false;
        int forestCount = 0;
        int forestColumn;
        forestColumn = Random.Range(2, xValue - 2);
        bool bird = false;
        lr = Random.Range(0, 2);
        for (int y = 0; y < yValue; y++)
        {
            for (int x = 0; x < xValue; x++)
            {
                switch (rows.ToArray()[y])
                {
                    case "Forest":
                        mAllCells[x, y].GetComponent<Image>().color = new Color32(0, 100, 0, 255);
                        if (((forestCount > 0 && forestCount < 4) && x == forestColumn) && row == false)
                        {
                            if (Random.Range(0, 2) == 0)
                            {
                                Bird b = new Bird(x, y, true, false, xValue, yValue);
                            }
                            else
                            {
                                Bird b = new Bird(xValue - 1, y, true, false, xValue, yValue);
                            }

                            bird = true;
                            treeArray.Add(x);
                            treeArray.Add(y);
                            forestColumn = Random.Range(2, xValue - 2);
                            GameObject newTree = Instantiate(mCellPrefab, transform);
                            treesList.Add(newTree);
                            RectTransform rectTransformA = newTree.GetComponent<RectTransform>();
                            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                            mAllAssets[x, y] = newTree.GetComponent<Cell>();
                            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                            mAllAssets[x, y].GetComponent<Image>().sprite = mTreeSprite;
                            row = true;
                        }

                        if (x == 0 || x == xValue - 1)
                        {
                            if (!bird)
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    Bird b = new Bird(x, y, true, false, xValue, yValue);
                                }
                                else
                                {
                                    Bird b = new Bird(xValue - 1, y, true, false, xValue, yValue);
                                }

                                bird = true;
                            }

                            treeArray.Add(x);
                            treeArray.Add(y);
                            GameObject newTree = Instantiate(mCellPrefab, transform);
                            treesList.Add(newTree);
                            RectTransform rectTransformA = newTree.GetComponent<RectTransform>();
                            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                            mAllAssets[x, y] = newTree.GetComponent<Cell>();
                            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                            mAllAssets[x, y].GetComponent<Image>().sprite = mTreeSprite;
                        }

                        if (x == xValue - 2)
                        {
                            if (forestCount == 4)
                            {
                                forestCount = 0;
                            }
                            else
                            {
                                forestCount++;
                            }
                        }

                        break;
                    case "Field":
                        mAllCells[x, y].GetComponent<Image>().color = new Color32(34, 139, 34, 255);
                        if (x == xValue - 1 && lr == 1 && row != true)
                        {
                            GameObject newGun = Instantiate(mCellPrefab, transform);
                            gunList.Add(newGun);
                            RectTransform rectTransformA = newGun.GetComponent<RectTransform>();
                            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                            mAllAssets[x, y] = newGun.GetComponent<Cell>();
                            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                            mAllAssets[x, y].GetComponent<Image>().sprite = mGunRight;
                            row = true;
                            lr = Random.Range(0, 2);
                            mGunArray.Add(new Gun(y, false, Random.Range(1, 4)));
                        }
                        else if (x == 0 && lr == 0 && row != true)
                        {
                            GameObject newGun = Instantiate(mCellPrefab, transform);
                            gunList.Add(newGun);
                            RectTransform rectTransformA = newGun.GetComponent<RectTransform>();
                            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                            mAllAssets[x, y] = newGun.GetComponent<Cell>();
                            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                            mAllAssets[x, y].GetComponent<Image>().sprite = mGunLeft;
                            row = true;
                            lr = Random.Range(0, 2);
                            mGunArray.Add(new Gun(y, true, Random.Range(1, 4)));
                        }

                        break;
                    case "River":
                        mAllCells[x, y].GetComponent<Image>().color = new Color32(0, 206, 209, 255);
                        break;
                    case "Road":
                        mAllCells[x, y].GetComponent<Image>().color = new Color32(105, 105, 105, 255);
                        break;
                    case "Plain":
                        mAllCells[x, y].GetComponent<Image>().color = new Color32(144, 238, 144, 255);
                        break;
                }
            }

            lr = Random.Range(0, 2);
            row = false;
            bird = false;
        }

        return treeArray;
    }

    public void GenerateCar(int xValue, int yValue, List<Road> roads)
    {
        List<GameObject> carsList2 = new List<GameObject>();
        int keep = 0;
        List<Car> cars;
        int i = 0;
        for (int y = 0; y < yValue; y++)
        {
            switch (rows.ToArray()[y])
            {
                case "Road":
                    cars = roads[i].GetCars();
                    for (int j = 0; j < cars.Count; j++)
                    {
                        for (int x = 0; x < xValue; x++)
                        {
                            if (x == cars[j].GetX())
                            {
                                keep++;
                                newCar = Instantiate(mCellPrefab, transform);
                                carsList.Add(newCar);
                                RectTransform rectTransformA = newCar.GetComponent<RectTransform>();
                                rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                                mAllAssets[x, y] = newCar.GetComponent<Cell>();
                                mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                                if (roads[i].GetDirection())
                                {
                                    mAllAssets[x, y].GetComponent<Image>().sprite = mCarRSprite;
                                }
                                else
                                {
                                    mAllAssets[x, y].GetComponent<Image>().sprite = mCarLSprite;
                                }
                            }
                        }
                    }

                    i++;
                    break;
            }
        }

        for (int j = 0; j < carsList.Count - keep; j++)
        {
            Destroy(carsList[j]);
        }

        for (int j = 0; j < carsList.Count; j++)
        {
            if (carsList[j] != null)
            {
                carsList2.Add(carsList[j]);
            }
        }

        carsList = carsList2;
    }

    public void GenerateBirds(int xValue, int yValue, List<Bird> mBirdArray1, List<int> mTreeArray)
    {
        mBirdArray = mBirdArray1;
        List<GameObject> birdsList2 = new List<GameObject>();
        int x2 = 0;
        int y2 = 0;
        int keep = 0;

        for (int i = 0; i < mBirdArray.Count; i++)
        {
            x2 = mBirdArray[i].GetX();
            y2 = mBirdArray[i].GetY();
            if (!mBirdArray[i].GetInTree())
            {
                mAllAssets = new Cell[xValue, yValue];
                GameObject newBird = Instantiate(mCellPrefab, transform);
                if (birdsList.Count < mBirdArray.Count)
                {
                    birdsList.Add(newBird);
                }
                else
                {
                    Destroy(birdsList[i]);
                    birdsList[i] = newBird;
                }

                RectTransform rectTransformA = newBird.GetComponent<RectTransform>();
                rectTransformA.anchoredPosition = new Vector2((x2 * 100) + 50, (y2 * 100) + 50);
                mAllAssets[x2, y2] = newBird.GetComponent<Cell>();
                mAllAssets[x2, y2].Setup(new Vector2Int(x2, y2), this);
                if (mBirdArray[i].GetImage())
                {
                    mAllAssets[x2, y2].GetComponent<Image>().sprite = birdImageR;
                }
                else
                {
                    mAllAssets[x2, y2].GetComponent<Image>().sprite = birdImageL;
                }
            }
            else
            {
                if (birdsList.Count != 0)
                {
                    Destroy(birdsList[i]);
                }
            }
        }
    }

    public void GenerateLogs(int xValue, int yValue, List<Logs> mLogArray1, Board mBoard)
    {
		int keep = 0;
        mLogArray = mLogArray1;
        int[] xCoordLog;
        Queue<int> rivers = new Queue<int>();
        int row;
        int lRowCount = 0;
        Queue<string> rowsA = new Queue<string>();
        mAllAssets = new Cell[xValue, yValue];
        rowsA = GetRows();
        for (int y = 0; y < yValue; y++)
        {
            switch (rowsA.ToArray()[y])
            {
                case "River":
                    rivers.Enqueue(y);
                    break;
            }
        }

        for (int i = 0; i < mLogArray.Count; i++)
        {
            row = mLogArray[i].GetRow();
            xCoordLog = mLogArray[i].GetXCoord();
            for (int j = 0; j < xCoordLog.Length; j++)
            {
                if (xCoordLog[j] >= 0 && xCoordLog[j] <= xValue - 1)
                {
                    GameObject newLog = Instantiate(mCellPrefab, transform);
                    logsList.Add(newLog);
                    keep++;
                    RectTransform rectTransformA = newLog.GetComponent<RectTransform>();
                    rectTransformA.anchoredPosition =
                        new Vector2((xCoordLog[j] * 100) + 50, (rivers.ToArray()[row] * 100) + 50);
                    mAllAssets[xCoordLog[j], rivers.ToArray()[row]] = newLog.GetComponent<Cell>();
                    mAllAssets[xCoordLog[j], rivers.ToArray()[row]]
                        .Setup(new Vector2Int(xCoordLog[j], rivers.ToArray()[row]), this);
                    mAllAssets[xCoordLog[j], rivers.ToArray()[row]].GetComponent<Image>().sprite = mLogSprite;
                }
            }
        }

        for (int j = 0; j < logsList.Count - keep; j++)
        {
            Destroy(logsList[j]);
            logsList.RemoveAt(j);
            j--;
        }
    }

    
    public List<Logs> GetmLogArray()
    {
        return mLogArray;
    }

    public Queue<string> GetRows()
    {
        return rows;
    }

    public List<Gun> GetGuns()
    {
        return mGunArray;
    }

    public bool CheckRiver(Player mPlayer)
    {
        int x = mPlayer.GetX();
        int y = mPlayer.GetMapY();
        int rowAdd = 0;
        bool found = true;
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

        if (rows.ToArray()[y] == "River")
        {
            for (int i = 0; i < mLogArray.Count; i++)
            {
                int[] xLog = mLogArray[i].GetXCoord();
                for (int j = 0; j < xLog.Length; j++)
                {
                    if (xLog[j] == x && (mLogArray[i].GetRow() + rowAdd) == y)
                    {
                        found = false;
                    }
                }
            }
        }
        else
        {
            found = false;
        }

        return found;
    }

    public void PowerUpCheck(int xPlayer, int yPlayer)
    {
        // Sprite sprite ;
        if (!powerUpOnBoard)
        {
            int x = Random.Range(1, 14);
            powerUpOnBoard = true;
            int type = Random.Range(0, 4);

            int y = 19;
            GameObject newPowerUp = Instantiate(mCellPrefab, transform);
            powerList.Add(newPowerUp);
            powerUp.SetX(x);
            powerUp.SetY(y);
            powerUp.SetType(type);
            Debug.Log(powerUp.GetType());
            switch (powerUp.GetType())
            {
                case 0:
                    Debug.Log("0");
                    sprite = mAngelSprite;
                    break;
                case 1:
                    Debug.Log("1");
                    sprite = mSlowSprite;
                    break;
                case 2:
                    Debug.Log("2");
                    sprite = mFreezeSprite;
                    break;
                case 3:
                    Debug.Log("3");
                    sprite = mBlinkSprite;
                    break;
            }

            RectTransform rectTransformA = newPowerUp.GetComponent<RectTransform>();
            rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
            mAllAssets[x, y] = newPowerUp.GetComponent<Cell>();
            mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
            mAllAssets[x, y].GetComponent<Image>().sprite = sprite;
        }
        else
        {
            if (xPlayer == powerUp.GetX() && yPlayer == powerUp.GetY())
            {
                switch (powerUp.GetType())
                {
                    case 0:
                        if (!powerUp.GetAngel())
                        {
                            powerUp.SetAngel();
                        }
                        break;
                    case 1:
                        if (!powerUp.GetSlow())
                        {
                            powerUp.SetSlow();
                            powerUp.SlowUp();
                        }
                        break;
                    case 2:
                        if (!powerUp.GetFreeze())
                        {
                            powerUp.SetFreeze();
                            powerUp.FreezeUp();
                        }
                        break;
                    case 3:
                        if (!powerUp.GetBlink())
                        {
                            powerUp.SetBlink();
                        }
                        break;
                }

                Debug.Log(powerUp.GetType());
                Destroy(powerList[0]);
                powerList.RemoveAt(0);
                powerUpOnBoard = false;
            }
            else
            {
                GameObject newPowerUp = Instantiate(mCellPrefab, transform);
                Destroy(powerList[0]);
                powerList.RemoveAt(0);
                powerList.Add(newPowerUp);
                if (powerUp.SetY(powerUp.GetY()))
                {
                    Destroy(powerList[0]);
                    powerList.RemoveAt(0);
                    powerList.Add(newPowerUp);
                    powerUpOnBoard = false;
                }
                else
                {
                    int x = powerUp.GetX();
                    int y = powerUp.GetY();
                    RectTransform rectTransformA = newPowerUp.GetComponent<RectTransform>();
                    rectTransformA.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                    mAllAssets[x, y] = newPowerUp.GetComponent<Cell>();
                    mAllAssets[x, y].Setup(new Vector2Int(x, y), this);
                    mAllAssets[x, y].GetComponent<Image>().sprite = sprite;
                }
            }
        }
    }

    public PowerUp GetPower()
    {
        return powerUp;
    }
}