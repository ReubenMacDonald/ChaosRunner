using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car
{
    private int x;
    public Car(int x1)
    {
        x = x1;
    }

    public int GetX()
    {
        return x;
    }

    public void Move(bool direct)
    {
        if (direct)
        {
            x++;
        }
        else
        {
            x--;
        }
    }
}