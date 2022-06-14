using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road
{
    private bool direction;
    public int xValue;
    public int volume;
    private List<Car> mCarArray;

    public Road(int direct, int xValue1, int volume1) //T-> || F <-
    {
        xValue = xValue1;
        volume = volume1;
        mCarArray = new List<Car>();
        if (direct == 1)
        {
            direction = false;
        }
        else
        {
            direction = true;
        }

        mCarArray = CheckCars(xValue,volume);
    }

    public List<Car> CheckCars(int xValue, int volume)
    {
        int difference;
        if (mCarArray.Count < volume)
        {
            difference = volume - mCarArray.Count;
            for (int i = 0; i < Random.Range(1, difference); i++)
            {
                if (direction)
                {
                    if (mCarArray.Count != 0 && mCarArray[mCarArray.Count - 1].GetX() < 1)
                    {
                        mCarArray.Add(new Car((mCarArray[mCarArray.Count - 1].GetX() - Random.Range(3, 7))));
                    }
                    else
                    {
                        mCarArray.Add(new Car(-Random.Range(3, 5)));
                    }
                }
                else
                {
                    if (mCarArray.Count != 0 && mCarArray[mCarArray.Count - 1].GetX() > xValue - 1)
                    {
                        mCarArray.Add(new Car((mCarArray[mCarArray.Count - 1].GetX() + Random.Range(3, 7))));
                    }
                    else
                    {
                        mCarArray.Add(new Car(xValue + Random.Range(3, 5)));
                    }
                }
            }
        }

        return mCarArray;
    }

    public void MoveCars(List<Car> mCarArray)
    {
        bool delete = false;

        for (int i = 0; i < mCarArray.Count; i++)
        {
            mCarArray[i].Move(GetDirection());
            if (GetDirection())
            {
                if (mCarArray[i].GetX() >= xValue)
                {
                    mCarArray.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                if (mCarArray[i].GetX() <= -1)
                {
                    mCarArray.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public bool GetDirection()
    {
        return direction;
    }
    public int GetVolume()
    {
        return volume;
    }

	public List<Car> GetCars()
    {
        return mCarArray;
    }
}
