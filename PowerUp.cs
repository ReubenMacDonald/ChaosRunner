using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PowerUp
{
    private bool angel = true;
    private bool slow = false;
    private bool freeze = false;
    private bool blink = false;
    private int slowCount = -1;
    private int freezeCount = -1;
    public int type = 0;
    private int x;
    private int y;

    public void Angel(Player mPlayer, Queue<string> rows)
    {
        SetAngel();
        mPlayer.SetAlive(true);
        int map = mPlayer.GetMapY();
        int place = map;
        while (rows.ToArray()[place]==rows.ToArray()[map])
        {
            place++;
        }

        place -= map;
        mPlayer.SetYAngel(place);
    }

    public int Slow()
    {
        return slowCount;
    }

    public void SlowUp()
    {
        slowCount++;
        if (slowCount > 10)
        {
            SetSlow();
            slowCount = -1;
        }
    }

    public int Freeze()
    {
        return freezeCount;
    }

    public void FreezeUp()
    {
        freezeCount++;
        if (freezeCount > 10)
        {
            SetFreeze();
            freezeCount = -1;
        }
    }

    public void Blink(Player mPlayer)
    {
        mPlayer.SetYAngel(1);
        SetBlink();
    }

    public void SetX(int x2)
    {
        x = x2;
    }

    public bool SetY(int y2)
    {
        if (y2==-1)
        {
            return true;
        }
        y = y2;
        return false;
    }

    public void SetType(int type2)
    {
        type = type2;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetType()
    {
        return type;
    }

    public void SetAngel()
    {
        angel = !angel;
        slow = false;
        freeze = false;
        blink = false;
    }

    public void SetSlow()
    {
        slow = !slow;
        angel = false;
        freeze = false;
        blink = false;
    }

    public void SetFreeze()
    {
        freeze = !freeze;
        slow = false;
        angel = false;
        blink = false;
    }

    public void SetBlink()
    {
        blink = !blink;
        slow = false;
        freeze = false;
        angel = false;
    }

    public bool GetAngel()
    {
        return angel;
    }

    public bool GetSlow()
    {
        return slow;
    }

    public bool GetFreeze()
    {
        return freeze;
    }

    public bool GetBlink()
    {
        return blink;
    }
}