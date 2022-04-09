using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    static bool canStack = false;

    public int duration;
    public bool toRemove;
    public CombatEntity target;

    public Effect(CombatEntity target, int duration) {
        this.target = target;
        this.duration = duration;
        toRemove = false;
    }

    //Needs to be called in order for effect to eventually wear off.
    public static bool RemoveCheck(Effect effect) {
        if (--effect.duration <= 0) {
            effect.OnEffectEnd();
            return true;
        }
        else { return false; }
    }


    public virtual void AffectOnTurn() { }
    public virtual Attack AffectOnHit(Attack atk) { return atk; }
    public virtual Attack AffectOnStrike(Attack atk) { return atk; }

    public virtual void OnEffectStart() { CombatLog.Instance.Log(target.entityName + " gained " + Name()); }
    public virtual void OnEffectEnd() { CombatLog.Instance.Log(Name() + " has worn off."); }

    public virtual bool CanStack() { return canStack; } 
    public virtual int MaxDuration() { return int.MaxValue; }
    public virtual string Name() { return "EFFECT"; }
    public override string ToString() {
        return $"{this.Name()} [ {this.duration} ]";
    }
}


#region Player Effects

public class MultiplyStrike : Effect {
    float strikeMultiplier;

    public MultiplyStrike(float multiplier, CombatEntity target, int duration) : base(target, duration) {
        strikeMultiplier = multiplier;
    }

    public override Attack AffectOnStrike(Attack atk) {
        atk.damage = (int)(atk.damage * strikeMultiplier);
        return atk;
    }

    public override string Name() { return "ANGER"; }
}

public class EvadePeriodic : Effect {
    protected int period;
    protected int current;

    public EvadePeriodic(int time, int startDelay, CombatEntity target, int duration) : base(target, duration) {
        this.period = time;
        this.current = startDelay;
    }

    public override Attack AffectOnHit(Attack atk) {
        if(--current <= 0) {
            current = period;
            atk.damage = 0;
            //atk.extra.Clear();
            CombatLog.Instance.Log(target.entityName + " avoids the attack");
        }
        return atk;
    }

    public override string Name() { return "FEAR"; }
}

public class GainExtra : Effect {
    int energyGain;
    int tensionGain;

    public GainExtra(int energyGain, int tensionGain, CombatEntity target, int duration) : base(target, duration) {
        this.energyGain = energyGain;
        this.tensionGain = tensionGain;
    }

    public override Attack AffectOnStrike(Attack atk) {
        atk.sender.GainTension(tensionGain);
        atk.sender.OnHeal(energyGain);
        return atk;
    }

    public override string Name() { return "ENVY"; }
}
#endregion



#region Enemy Effects

public class Telegraph : Effect {
    public Telegraph(CombatEntity target, int duration) : base(target, duration) { }
    public override void OnEffectStart() {
        CombatLog.Instance.Log(target.entityName + " is charging something big!");
    }
    public override void OnEffectEnd() { }
    public override string Name() { return "CHARGING"; }
}

public class MultiplyStrikeInOut : MultiplyStrike {
    float hitMultiplier;
    public MultiplyStrikeInOut(float multiplyIn, float multiplyOut, CombatEntity target, int duration) : base(multiplyOut, target, duration) {
        hitMultiplier = multiplyIn;
    }

    public override Attack AffectOnHit(Attack atk) {
        atk.damage = (int)(atk.damage * hitMultiplier);
        return atk;
    }

    public override string Name() { return "WEAK"; }
}

public class SendBackDamage : Effect {
    float multiplier;

    public SendBackDamage(float multiplier, CombatEntity target, int duration) : base(target, duration) {
        this.multiplier = multiplier;
    }

    public override Attack AffectOnHit(Attack atk) {
        target.OnHit(atk, false);
        CombatLog.Instance.Log("Damage applies to all linked entities!");

        return atk;
    }

    public override void OnEffectStart() {
        CombatLog.Instance.Log("Both parties are linked!");
    }
    public override string Name() { return "LINKED"; }
}

public class SendBackDamageNoLog : SendBackDamage {
    public SendBackDamageNoLog(float multiplier, CombatEntity target, int duration) : base(multiplier, target, duration) { }

    public override void OnEffectStart() { }
    public override void OnEffectEnd() { }
}

#endregion