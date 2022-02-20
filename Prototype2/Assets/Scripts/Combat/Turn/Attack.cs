using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class Attack {
    public int damage;
    public CombatEntity target;
    public CombatEntity sender;
   // public List<VoidStrategy> extra = new List<VoidStrategy>();
}

//For defining extension methods that can be used in 'extra' list.
public static partial class AttackBehavior { }
