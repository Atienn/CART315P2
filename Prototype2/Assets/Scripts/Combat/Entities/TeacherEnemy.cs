using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Misc;
using System;
using UnityEngine.Events;

public class TeacherEnemy : Enemy 
{
    [Header("Mentor")]
    [SerializeField] Message[] objectiveMsgs = { };
    BoolStrategy[] objectiveChecks;

    [SerializeField] TMP_Text objectiveMain;
    [SerializeField] TMP_Text objectiveNote;
    [SerializeField] Button resumebutton;
    [Space]
    [SerializeField] UnityEvent onComplete;
    [SerializeField] UnityEvent onIncomplete;


    int i;

    bool wasHit;
    int lastEnergy;
    int lastTension;

    bool attacks;

    protected override void Start() {
        base.Start();
        objectiveChecks = new BoolStrategy[] { Free, Free, HasStruck, Free, HasCast, Free, Free, HasHealed, Free, Free, StartBalanceTest, IsBalancePositive };
        attacks = false;
        i = 0;
    }

    public override void Act() {

        if(i < objectiveChecks.Length) {

            if (objectiveChecks[i]()) {
                objectiveMain.text = objectiveMsgs[i].main;
                objectiveNote.text = objectiveMsgs[i].note;
                i++;

                CombatAudio.Instance.PauseAudio();
                resumebutton.gameObject.SetActive(true);
            }
        }

        if(attacks) {
            Attack atk = new Attack {
                damage = strikePow,
                sender = this,
                target = combat.player
            };

            this.OnStrikeSend(atk);
        }
        else {
            CombatLog.Instance.Log(entityName + " waits.");
        }

        wasHit = false;
        lastEnergy = combat.player.energy;
        lastTension = combat.player.tension;
    }

    public override void OnHit(Attack receiving, bool applyEffects = true) {
        base.OnHit(receiving, applyEffects);
        wasHit = true;
    }


    bool Free() { return true; }
    bool HasStruck() { return wasHit; }
    bool HasCast() {
        return (lastTension > combat.player.tension) && (lastEnergy >= combat.player.energy); }
    bool HasHealed() { return (lastEnergy + 10 < combat.player.energy) || combat.player.energy >= combat.player.maxEnergy; }
    bool IsBalancePositive() { return combat.balanceGauge.target > 0; }
    bool StartBalanceTest() {
        attacks = true;
        combat.OffsetBalance(-200);
        return true;
    }


    public void OnEnd() {
        if(i >= objectiveChecks.Length) {
            SetCleared();
            onComplete.Invoke();
        }
        else {
            onIncomplete.Invoke();
        }
    }

    [Serializable]
    struct Message {
        public string main;
        public string note;
    }
}
