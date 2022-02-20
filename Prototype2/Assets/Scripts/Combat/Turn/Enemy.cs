using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatEntity
{
    [SerializeField] ImageGauge trackGauge;

    protected override void Start() {
        base.Start();
        trackGauge.maxValue = combat.music.clip.length;
    }

    private void FixedUpdate() {
        trackGauge.SetTargetOnly(trackGauge.maxValue - combat.music.time);
    }

    public override void Act() {

        Attack atk = new Attack {
            damage = strikePow,
            sender = this,
            target = combat.player
        };

        this.OnStrikeSend(atk);
    }

    public override void OnHit(Attack receiving) {
        base.OnHit(receiving);
        combat.OffsetDominant(receiving.damage);
    }
}
