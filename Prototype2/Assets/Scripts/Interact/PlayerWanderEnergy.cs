using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWanderEnergy : Singleton<PlayerWanderEnergy>
{
    [SerializeField] ImageGauge energyGauge;

    int healDelay;
    int currDelay;

    protected override void Start() {
        base.Start();

        energyGauge.maxValue = Blackboard.PlayerInfo.maxEnergy;
        energyGauge.SetTargetAndText(Blackboard.PlayerInfo.energy);
        
        enabled = false;
    }

    void FixedUpdate() {
        if(currDelay-- <= 0) {
            currDelay = healDelay;

            Blackboard.PlayerInfo.SaveEnergy(Blackboard.PlayerInfo.energy + 1);
            energyGauge.SetTargetAndText(Blackboard.PlayerInfo.energy);

            //Stop healing if at max.
            if (Blackboard.PlayerInfo.energy >= Blackboard.PlayerInfo.maxEnergy) {
                enabled = false;
            }
        }
    }

    public void HealOverTime(int delay) {
        //Only enable if not at max energy.
        if (Blackboard.PlayerInfo.energy < Blackboard.PlayerInfo.maxEnergy) {
            healDelay = delay;
            currDelay = healDelay;
            enabled = true;
        }
    }
    public void StopHealOverTime() {
        enabled = false;
    }

    public void HealOverTimeCleared(int id, int amount) {
        if (Blackboard.Progress.combatClear[id]) {
            PlayerWanderEnergy.Instance.HealOverTime(amount);
        }
    }

    public void HealDirect(int amount) {
        Blackboard.PlayerInfo.SaveEnergy(Blackboard.PlayerInfo.energy + amount);
        energyGauge.SetTargetAndText(Blackboard.PlayerInfo.energy);
    }
}
