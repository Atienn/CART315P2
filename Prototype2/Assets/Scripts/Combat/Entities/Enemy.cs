using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : CombatEntity
{
    [SerializeField] ImageGauge energyGauge;
    

    protected override void Start() {
        base.Start();
        energyGauge.maxValue = this.maxEnergy;
        energyGauge.SetTargetAndText(this.energy);
    }

    public override void Act() { }

    public override void OnHit(Attack receiving) {
        base.OnHit(receiving);

        energyGauge.SetTargetAndText(this.energy);
        combat.OffsetBalance(receiving.damage);
    }
}
