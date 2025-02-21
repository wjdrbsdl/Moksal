﻿using System.Collections;
using UnityEngine;


public class CharPlayer : CharactorBase
{
    public PlayerMove m_playerMove;

    public void SetTarget(Vector3 _pos)
    {
        m_playerMove.SetTarget(_pos);
    }
}
