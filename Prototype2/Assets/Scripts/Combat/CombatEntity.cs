using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CombatEntity : MonoBehaviour {

    [SerializeField] protected TurnManager combat;

    [Space]
    public string entityName;
    protected int energy;
    protected int tension;

    [Space]
    [SerializeField] protected int maxEnergy;
    [Space]
    [SerializeField] protected int maxTension;
    [SerializeField] protected int tensionGain;
    [Space]
    [SerializeField] protected int strikePow;

    public Image render;
    [SerializeField] protected TextList effectTextList;

    //Should ideally be a priority queue.
    protected List<Effect> statusEffects = new List<Effect>();


    protected virtual void Start() {
        this.energy = maxEnergy;
        this.tension = 0;
    }

    public abstract void Act();


    public virtual void OnTurn() {
        this.statusEffects.ForEach(e => e.AffectOnTurn());
        //Remove all effects whose timer has reached 0.
        this.statusEffects.RemoveAll(Effect.RemoveCheck);

        //Refresh the effect list.
        effectTextList.ClearDisplay();
        statusEffects.ForEach(e => effectTextList.AddLast(e.ToString()));
    }

    public virtual void OnStrikeSend(Attack sending) {
        //Modify the attack with all effects that apply.
        this.statusEffects.ForEach(e => e.AffectOnStrike(sending));

        sending.target.OnHit(sending);
    }
    public virtual void OnStrikeSuccess() {
        GainTension(this.tensionGain);
    }
    public virtual void OnStrikeFail() { }

    public virtual void OnHit(Attack receiving) {
        //Modify the attack with all effects that apply.
        this.statusEffects.ForEach(e => e.AffectOnHit(receiving));

        this.energy -= receiving.damage;
        receiving.sender.OnStrikeSuccess();

        StartCoroutine(Flash(2));
        CombatLog.Instance.Log($"{receiving.sender.entityName} strikes {this.entityName} for {receiving.damage}.");
    }
    //Should have an OnStrikeSuccess() event instead.


    public virtual void OnHeal(int amount) {
        this.energy += amount;
        if (this.energy > this.maxEnergy) {
            this.energy = maxEnergy;
        }
        CombatLog.Instance.Log($"{this.entityName} heals of {amount}.");
    }

    public void GainTension(int amount) {
        this.tension += amount;
        if (this.tension > this.maxTension) {
            this.tension = this.maxTension;
        }
    }

    public bool SpendTension(int amount) {
        if (this.tension < amount) {
            return false;
        }
        this.tension -= amount;
        return true;
    }


    #region Effects
    //Return true if the effect wasn't already in the list.
    public bool AddEffect(Effect newEffect) {
        if(!newEffect.CanStack()) {

            //If the effect can't stack, check if it's already exists in the list.
            //If it is, extend the duration.
            foreach (Effect effect in statusEffects) {

                //Check if any effect is of the same type (by checking if it's of the same C# class).
                if (effect.GetType() == newEffect.GetType()) {

                    //Give the effect either the sum of durations or the max duration, whichever is smaller.
                    effect.duration = Mathf.Min(effect.duration + newEffect.duration, newEffect.MaxDuration());

                    //Refresh the effect list.
                    effectTextList.ClearDisplay();
                    statusEffects.ForEach(e => effectTextList.AddLast(e.ToString()));
                    return false;
                }
            }
        }

        //If the effect wasn't found or it can stack, add it to the list.
        statusEffects.Add(newEffect);
        newEffect.OnEffectStart();
        return true;
    }

    public bool RemoveEffect(Effect eff) {
        return statusEffects.Remove(eff);
    }

    #endregion

    IEnumerator<WaitForSeconds> Flash(int amount) {
        for(int i = 0; i < amount; i++) {
            render.enabled = false;
            yield return new WaitForSeconds(0.15f);
            render.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
