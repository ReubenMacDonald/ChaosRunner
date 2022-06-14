using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logs
{
    private int[] xLog;
    private int logSize;
    private int row = 0;
    private int mapRow = 0;
    private bool direction = false;
    private int boardWidth;
    private int randomSeed;
    private int xValue;
    private int yValue;
    private bool gameStart;
    public Queue<string> rows = new Queue<string>();
    private int logNo = 0;
    private List<Logs> mLogArray;

    public Logs()
    {
    }
//Direction - True = -> | False = <-
    public Logs(int logSize1, int logRow, bool logDirection, int boardWidth1)
    {
        int reverseI = 0;
        logSize = logSize1;
        boardWidth = boardWidth1;
        xLog = new int[logSize];
        mLogArray = new List<Logs>();
        row = logRow;
        direction = logDirection;
        for (int i = 0; i < logSize; i++)
        {
            if (direction)
            {
                xLog[i] = reverseI;
                reverseI--;
            }
            else
            {
                xLog[i] = boardWidth + i - 1;
            }
        }
    }

    public void InitValues(int randomSeed1, int xValue1, int yValue1, bool gameStart1,
        List<Logs> mLogArray1)
    {
        mLogArray = mLogArray1;
        randomSeed = randomSeed1;
        Random.InitState(randomSeed);
        xValue = xValue1;
        yValue = yValue1;
        gameStart = gameStart1;
    }

    public List<Logs> GetLogArray()
    {
        return mLogArray;
    }
    
    public List<Logs> CreateLogs(Board mBoard, List<Logs> mLogArray1, int yValue, int xValue)
    {
        mLogArray = mLogArray1;
        logNo = 0;
        rows = mBoard.GetRows();
        int logSize;
        int logRow;
        int logDirect;
        int difference;
        int count;
        int[] logX;
        bool check = true;
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

        mapRow = rowAdd + row;
        for (int x = 0; x < yValue; x++)
        {
            if (rows.ToArray()[x] == "River")
            {
                logNo++;
            }
        }

        if (gameStart)
        {
            for (int i = 0; i < logNo; i++)
            {
                logSize = Random.Range(2, 5);
                logDirect = Random.Range(0, 2);
                if ((logDirect == 1 && i % 5 != 1) || i % 5 == 0)
                {
                    mLogArray.Add(new Logs(logSize, i, true, xValue));
                }
                else
                {
                    mLogArray.Add(new Logs(logSize, i, false, xValue));
                }
            }
        }
        else
        {
            difference = LogDifference(logNo);
            difference = Random.Range(1, difference);
            for (int i = 0; i < difference * 2; i++)
            {
                count = mLogArray.Count;
                logSize = Random.Range(2, 5);
                logDirect = Random.Range(0, 2);
                logRow = Random.Range(0, logNo);
                for (int j = 0; j < count; j++)
                {
                    if (logRow == mLogArray[j].GetRow())
                    {
                        logX = mLogArray[j].GetXCoord();
                        for (int k = 0; k < logX.Length; k++)
                        {
                            if ((logDirect == 1 && logRow % 5 != 1) || logRow % 5 == 0)
                            {
                                if (logX[k] == 0 || logX[k] == -1 || logX[k] == 1)
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (logX[k] == xValue || logX[k] == xValue - 1 || logX[k] == xValue - 2)
                                {
                                    check = false;
                                }
                            }
                        }
                    }
                }


                if (check)
                {
                    if ((logDirect == 1 && logRow % 5 != 1) || logRow % 5 == 0)
                    {
                        mLogArray.Add(new Logs(logSize, logRow, true, xValue));
                        count = mLogArray.Count;
                    }
                    else
                    {
                        mLogArray.Add(new Logs(logSize, logRow, false, xValue));
                        count = mLogArray.Count;
                    }
                }
                else
                {
                    check = true;
                }
            }
        }
        return mLogArray;
    }

    public int LogDifference(int logNo)
    {
        int difference = 0;
        if (logNo * 2 > mLogArray.Count)
        {
            difference = logNo * 2;
        }

        return difference;
    }

    public int GetRow()
    {
        return row;
    }public int GetMRow()
    {
        return mapRow;
    }

    public void DownRow()
    {
        row--;
        mapRow--;
    }

    public int[] GetXCoord()
    {
        return xLog;
    }

    public bool GetDirection()
    {
        return direction;
    }

    public bool MoveLog(int[] xLog, bool direct) //True -> False <-
    {
        if (direct)
        {
            if (xLog[xLog.Length-1] != boardWidth - 1)
            {
                for (int i = 0; i < xLog.Length; i++)
                {
                    xLog[i] = xLog[i] + 1;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (xLog[xLog.Length - 1] != 0)
            {
                for (int i = 0; i < xLog.Length; i++)
                {
                    xLog[i] = xLog[i] - 1;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void CheckCollision()
    {
        bool delete = false;
        int[] xLogs1;
        int[] xLogs2;
        for (int i = 0; i < mLogArray.Count; i++)
        {
            for (int j = 0; j < mLogArray.Count; j++)
            {
                if (i != j)
                {
                    if (mLogArray[i].GetRow() == mLogArray[j].GetRow())
                    {
                        xLogs1 = mLogArray[i].GetXCoord();
                        xLogs2 = mLogArray[j].GetXCoord();
                        for (int k = 0; k < xLogs1.Length; k++)
                        {
                            for (int l = 0; l < xLogs2.Length; l++)
                            {
                                if (xLogs1[k] == xLogs2[l])
                                {
                                    delete = true;
                                }
                            }
                        }
                    }

                    if (delete)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                mLogArray.RemoveAt(i);
                                i--;
                                break;
                            case 1:
                                mLogArray.RemoveAt(j);
                                j--;
                                break;
                        }
                        delete = false;
                    }
                }
            }
        }
    }
}