using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public delegate void VoidStrategy();
    public delegate bool BoolStrategy();

    public delegate void VectorParamStrategy(Vector3 v);

    public static class StandardMethods
    {
        public static void None() { }
    }
}
