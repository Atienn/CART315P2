using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementControl : MonoBehaviour
{
    public abstract Vector3 GetDirection();
    public abstract bool GetAction1();
    public abstract bool GetAction2();
}
