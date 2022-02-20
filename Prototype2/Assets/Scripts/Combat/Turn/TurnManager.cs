using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    bool turn = false;

    public CombatEntity player;
    public CombatEntity enemy;
    [SerializeField] SliderGauge balanceGauge;

    public AudioSource music;
    [SerializeField] AudioEventTimer turnTimer;
    [SerializeField] AudioEventTimer beatTimer;
    [SerializeField] GameObject[] beats = new GameObject[4];

    [SerializeField] UnityEvent onWin;
    [SerializeField] UnityEvent onLose;


    void Start() {
        
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

                beatTimer.AddEventBack((beats.Length - i) * beatLength + turnTimer.source.timeSamples, tempEvent);
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
                beatTimer.AddEventBack((i + 1) * beatLength + turnTimer.source.timeSamples, tempEvent);
            }
            player.OnTurn();
        }
    }

    public void OffsetDominant(int offset) {
        balanceGauge.SetTarget(balanceGauge.target + offset);
    }



    public void CombatEndDeath() {

        StartCoroutine(StopMusic());

        turnTimer.enabled = false;
        turnTimer.events.Clear();
        beatTimer.enabled = false;
        beatTimer.events.Clear();

        player.render.gameObject.SetActive(false);

        onLose.Invoke();
    }

    IEnumerator<WaitForFixedUpdate> StopMusic() {
        while(music.pitch > 0) {
            music.pitch = Mathf.MoveTowards(music.pitch, 0, 0.02f);
            yield return new WaitForFixedUpdate();
        }
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
        enemy.render.enabled = false;
        CombatLog.Instance.Log(player.entityName + " wins.");
        onWin.Invoke();
    }
}