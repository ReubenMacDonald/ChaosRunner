using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    private IObserver deathObserver;

    public void SetObs(IObserver obs)
    {
        deathObserver = obs;
    }

    public void Notify(string death, PlayerManager mPlayerManager, GameObject mImage, Sprite mBird, Sprite mCar,
        Sprite mDrown, Sprite mGun, GameObject deadPanel, GameObject canvas, GameManager gm)
    {
        deathObserver.OnNotify(death, mPlayerManager, mImage, mBird, mCar, mDrown, mGun, deadPanel, canvas, gm);
    }
}
