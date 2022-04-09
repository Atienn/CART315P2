using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeStrikeEnemy : Enemy {
    bool castReady = false;


    public override void Act() {

        //Use ability if ready.
        if (castReady) {
            castReady = false;

            CombatLog.Instance.Log($"{entityName} launches a super-strike!");
            Attack super = new Attack {
                damage = 3 * strikePow,
                sender = this,
                target = combat.player
            };

            OnStrikeSend(super);
        }

        //Charge if have enough tension.
        else if (tension >= 20) {
            tension -= 20;

            AddEffect(new Telegraph(this, 1));
            castReady = true;
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
