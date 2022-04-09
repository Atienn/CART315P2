using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : Singleton<TurnManager>
{
    bool turn = false;

    public CombatEntity player;
    public CombatEntity enemy;
    public SliderGauge balanceGauge;
    [SerializeField] ImageGauge trackGauge;

    [SerializeField] AudioEventTimer turnTimer;
    [SerializeField] AudioEventTimer beatTimer;
    [SerializeField] GameObject[] beats = new GameObject[4];

    [SerializeField] UnityEvent onWin;
    [SerializeField] UnityEvent onLose;

    protected override void Start() {
        base.Start();
        trackGauge.maxValue = CombatAudio.Instance.source.clip.length;
    }

    public void TurnChange(bool next) {
        turn = next;

        int beatLength = (turnTimer.untilNext) / 4;
        UnityEvent tempEvent;

        if (next) {
            player.Act();
            CombatLog.Instance.gameObject.SetActive(true);

            int i = beats.Length - 1;
            int mid = i / 2 + 1;
            for (; i >= 0; --i) {
                tempEvent = new UnityEvent();

                //Required as not to capture variable i which will change.
                short j = (short)i;
                tempEvent.AddListener(delegate { beats[j].SetActive(true); });

                if (i == mid) {
                    tempEvent.AddListener(enemy.Act);
                }

                beatTimer.AddEventBack((beats.Length - i) * beatLength + CombatAudio.Instance.source.timeSamples, tempEvent);
            }
            enemy.OnTurn();
        }
        else {
            CombatLog.Instance.Clear();
            CombatLog.Instance.gameObject.SetActive(false);


            for (short i = 0; i < beats.Length; ++i) {
                tempEvent = new UnityEvent();

                //Required as not to capture variable i which will change.
                short j = i;
                tempEvent.AddListener(delegate { beats[j].SetActive(false); });
                beatTimer.AddEventBack((i + 1) * beatLength + CombatAudio.Instance.source.timeSamples, tempEvent);
            }
            player.OnTurn();
        }

        trackGauge.SetTargetOnly(trackGauge.maxValue - CombatAudio.Instance.source.time);
    }

    public void OffsetBalance(int offset) {
        balanceGauge.SetTarget(balanceGauge.target + offset);
    }



    public void CombatEndDeath() {

        CombatAudio.Instance.PauseAudio();

        turnTimer.enabled = false;
        turnTimer.events.Clear();
        beatTimer.enabled = false;
        beatTimer.events.Clear();

        player.render.gameObject.SetActive(false);

        onLose.Invoke();
    }



    public void CombatEnd() {
        if(balanceGauge.target > 0) { CombatEndWin(); }
        else { CombatEndLose(); }
    }

    public void CombatEndLose() {
        player.render.enabled = false;
        CombatLog.Instance.Log(enemy.entityName + " wins.");
        onLose.Invoke();
    }

    public void CombatEndWin() {
        SavePlayerEnergy();
        enemy.render.enabled = false;
        CombatLog.Instance.Log(player.entityName + " wins.");
        onWin.Invoke();
    }

    public void SavePlayerEnergy() {
        Blackboard.PlayerInfo.energy = player.energy;
    }
}