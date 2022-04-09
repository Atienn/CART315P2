using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : CombatEntity
{
    [Header("Enemy")]
    [SerializeField] int enemyId;
    [SerializeField] ImageGauge energyGauge;

    protected override void Start() {
        base.Start();
        energyGauge.maxValue = this.maxEnergy;
        energyGauge.SetTargetAndText(this.energy);
    }

    public override void Act() { }

    public override void OnHit(Attack receiving, bool applyEffects = true) {
        base.OnHit(receiving, applyEffects);

        energyGauge.SetTargetAndText(this.energy);
        combat.OffsetBalance(receiving.damage);

        if (this.energy <= 0) {
            combat.CombatEndWin();
        }
    }

    protected void SetCleared() {
        Blackboard.Progress.combatClear[enemyId] = true;
    }
}
