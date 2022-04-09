using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tracks values that should carry over between scenes.
namespace Blackboard {

    public static class PlayerInfo {
        public static Vector3 position = new Vector3(-330, -189, 0);
        public static int energy = 12;
        public static int maxEnergy = 75;

        /// <summary> Only needed for events. For code, set value directly. </summary>
        public static void SavePosition(Vector3 newPos) {
            position = newPos;
        }

        /// <summary> Return if now at max energy. </summary>
        public static void SaveEnergy(int newEnergy) {
            if(newEnergy > maxEnergy) { energy = maxEnergy; }
            else {  energy = newEnergy; }
        }
    }

    public static class Progress {
        public static bool[] combatClear = new bool[2];
    }
}