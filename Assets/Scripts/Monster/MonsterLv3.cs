using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLv3 : Monster
{
    public override void Initialized()
    {
        damage = 2;
        curHealth = 3;
    }
}
