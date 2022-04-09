using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkerEnemy : Enemy
{
    bool secondChance = true;
    bool castReady = false;
    bool castChoice = false;

    override protected void Start() {
        base.Start();

        secondChance = true; 
    }

    public override void Act() {

        //Use ability if ready.
        if(castReady) {
            castReady = false;
            castChoice = !castChoice;

            if (castChoice) {
                AddEffect(new SendBackDamageNoLog(1, combat.player, 2));
                combat.player.AddEffect(new SendBackDamage(1, this, 3));
            }
            else {
                combat.OffsetBalance(-115);
                CombatLog.Instance.Log($"{entityName} cheats the balance!");
            }
        }

        //Charge if have enough tension.
        else if(tension >= 40) {
            tension -= 40;

            AddEffect(new Telegraph(this, 1));
            castReady = true;
        }

        //Let the player live once.
        else if(secondChance && combat.player.energy < strikePow) {
            secondChance = false;
            this.tension += 2 * tensionGain;
            CombatLog.Instance.Log($"{entityName} goes easy on you!");
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
