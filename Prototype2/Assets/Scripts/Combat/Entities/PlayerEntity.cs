using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class PlayerEntity : CombatEntity {

    [SerializeField] float critChance;
    //float defaultCritChance;

    [SerializeField] GameObject actions;
    [SerializeField] ImageGauge energyGauge;
    [SerializeField] ImageGauge tensionGauge;

    VoidStrategy[] actStrategies;
    int actionIndex;

    override protected void Start() {
        base.Start();
        //                             0       1          2         3         4             5           6
        actStrategies = new VoidStrategy[] { Strike, CastAnger, CastFear, CastEnvy, HealRational, HealDenial, GiveUp };

        energy = Blackboard.PlayerInfo.energy;

        energyGauge.maxValue = this.maxEnergy;
        tensionGauge.maxValue = this.maxTension;
        energyGauge.SetTargetAndText(this.energy);
        tensionGauge.SetTargetAndText(this.tension);
    }

    public void SetActionIndex(int i) {
        actionIndex = i;
    }

    public override void Act() {
        actions.SetActive(false);
        actStrategies[actionIndex]();
    }


    #region Game Events
    public override void OnTurn() {
        base.OnTurn();
        actions.SetActive(true);
    }

    public override void OnHit(Attack receiving, bool applyEffects = true) {
        base.OnHit(receiving, applyEffects);
        energyGauge.SetTargetAndText(this.energy);
        combat.OffsetBalance(-receiving.damage);
        
        if(this.energy <= 0) {
            combat.CombatEndDeath();
        }
    }
    public override void OnStrikeSuccess() {
        base.OnStrikeSuccess();
        tensionGauge.SetTargetAndText(this.tension);
    }
    public override void OnHeal(int amount) {
        base.OnHeal(amount);
        energyGauge.SetTargetAndText(this.energy);
    }
    #endregion


    #region Actions

    void Strike() {
        Attack atk = new Attack { 
            damage = strikePow,
            sender = this,
            target = combat.enemy
        };

        this.OnStrikeSend(atk);
    }

    #region Casts

    void TryCast(int tensionCost, Effect effect) {
        if (SpendTension(tensionCost)) {
            this.AddEffect(effect);
            tensionGauge.SetTargetAndText(this.tension);
        }
        else {
            CombatLog.Instance.Log("Not enough tension. Striking instead");
            Strike();
        }
    }

    void CastAnger() {
        TryCast(30, new MultiplyStrike(2, this, 7));
    }
    void CastFear() {
        TryCast(45, new EvadePeriodic(2, 0, this, 4));
    }
    void CastEnvy() {
        TryCast(20, new GainExtra(10, 6, this, 6));
    }
    #endregion //Casts

    #region Heals

    void TryHeal(int tensionCost, int amount) {
        if (SpendTension(tensionCost)) {
            OnHeal(amount);
            tensionGauge.SetTargetAndText(this.tension);
        }
        else {
            CombatLog.Instance.Log("Not enough tension. Striking instead.");
            Strike();
        }
    }

    void HealRational() {
        //Heal 50% of missing energy.
        TryHeal(25, (this.maxEnergy - this.energy) / 2);
    }
    void HealDenial() {
        TryHeal(40, (3 * this.maxEnergy / 5));
    }
    #endregion //Heals

    void GiveUp() {
        Attack giveUp = new Attack {
            damage = ushort.MaxValue,
            sender = this,
            target = this
        };
        
        CombatLog.Instance.Log(this.entityName + " gave up.");

        OnHit(giveUp);
    }

    #endregion
}
