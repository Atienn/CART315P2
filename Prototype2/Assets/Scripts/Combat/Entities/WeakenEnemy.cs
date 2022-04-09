using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenEnemy : Enemy
{
    public override void Act() {

        if (tension >= 35) {
            tension -= 35;

            combat.player.AddEffect(new MultiplyStrikeInOut(1.5f, 0.75f, combat.player, 3));
        }
        //Attack normally.
        else {
            Attack atk = new Attack {
                damage = strikePow,
                sender = this,
                target = combat.player
            };

            OnStrikeSend(atk);
        }
    }
}
