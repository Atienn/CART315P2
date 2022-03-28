using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : CombatEntity
{
    [SerializeField] ImageGauge trackGauge;
    

    protected override void Start() {
        base.Start();
        //trackGauge.maxValue = CombatAudio.Instance.source.clip.length;
    }

    public override void Act() {

    }

    private void FixedUpdate() {
        //trackGauge.SetTargetOnly(trackGauge.maxValue - combat.music.time);
    }

    public override void OnHit(Attack receiving) {
        base.OnHit(receiving);
        combat.OffsetBalance(receiving.damage);
    }
}
