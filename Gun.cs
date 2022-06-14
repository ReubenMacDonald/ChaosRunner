using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    private int y;
    private bool direction;
    private int reload;

    public Gun(int y1, bool direction1, int reload1) //stand right = false
    {
        y = y1;
        reload = reload1*4;
        direction = direction1;
    }

    public void Down()
    {
        y--;
    }

    public bool GetDirect()
    {
        return direction;
    }

    public int GetY()
    {
        return y;
    }

    public bool Shoot(int count)
    {
        if (count % reload == 0)
        {
            return true;
        }

        return false;
    }
}