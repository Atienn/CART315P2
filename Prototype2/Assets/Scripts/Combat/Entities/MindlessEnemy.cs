using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindlessEnemy : Enemy
{
    public override void Act() {
        Attack atk = new Attack {
            damage = strikePow,
            sender = this,
            target = combat.player
        };

        this.OnStrikeSend(atk);
    }
}
