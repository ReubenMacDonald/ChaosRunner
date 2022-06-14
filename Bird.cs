using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird
{
    private int x;
    private int y;
    private int xValue;
    private int yValue;
    private bool inTree;
    private bool direct;
    public List<Bird> mBirdArray;
    public List<int> mTreeArray;

    public Bird(List<int> trees)
    {
        mTreeArray = trees;
    }

    public Bird(int randomSeed, List<int> trees)
    {
        mTreeArray = trees;
        mBirdArray = new List<Bird>();
        Random.InitState(randomSeed);
    }

    public Bird(int xIn, int yIn, bool inTrees, bool directT, int xValues, int yValues)
    {
        x = xIn;
        y = yIn;
        xValue = xValues;
        yValue = yValues;
        inTree = inTrees;
        direct = directT;
    }

    public bool GetImage()
    {
        return direct;
    }

    public Bird(int xIn, int yIn, bool inTrees, bool directT, int xValues, int yValues, List<Bird> birdArray)
    {
        x = xIn;
        y = yIn;
        inTree = inTrees;
        direct = directT;
        mBirdArray = birdArray;
        mBirdArray.Add(new Bird(x, y, inTree, direct, xValues, yValues));
    }

    public List<Bird> CreateBirds(int xValues, int yValues)
    {
        int x = 0;
        int y = 0;
        int lasty = -1;
        bool bird = false;
        for (int i = 0; i < mTreeArray.Count; i++)
        {
            x = mTreeArray[i];
            y = mTreeArray[i + 1];
            i++;
            if (lasty != y && bird != true)
            {
                bird = false;
                lasty = y;
                if (Random.Range(0, 2) == 0)
                {
                    mBirdArray.Add(new Bird(x, y, true, false, xValues, yValues));
                    bird = true;
                }
                else
                {
                    mBirdArray.Add(new Bird(xValues - 1, y, true, false, xValues, yValues));
                    bird = true;
                }
            }
            else
            {
                bird = false;
            }
        }
        return mBirdArray;
    }

    public void MoveBird(List<int> mTreeArray)
    {
        int dir = Random.Range(0, 2);
        if (inTree)
        {
            if ((dir == 0 || x == 0) && x < xValue - 1)
            {
                direct = true;
            }
            else
            {
                direct = false;
            }
        }

        if (direct)
        {
            x++;
            inTree = false;
        }
        else
        {
            x--;
            inTree = false;
        }

        for (int i = 0; i < mTreeArray.Count; i++)
        {
            if (x == mTreeArray[i] && y == mTreeArray[i + 1])
            {
                inTree = true;
            }

            i++;
        }
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public void DecreaseY()
    {
        y--;
    }

    public bool GetDirect()
    {
        return direct;
    }

    public bool GetInTree()
    {
        return inTree;
    }

    public List<Bird> GetBirdArray()
    {
        return mBirdArray;
    }

    public List<int> GetTreeArray()
    {
        return mTreeArray;
    }
}
