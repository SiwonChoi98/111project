using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLv1 : Monster
{
    public override void Initialized()
    {
        damage = 1;
        curHealth = 1;
    }
}
