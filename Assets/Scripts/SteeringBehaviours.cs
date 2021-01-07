using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteeringBehaviours
{
    public static Vector3 Flee(Vector3 agentPos,Vector3 point)
    {
        agentPos.y = 0;
        point.y = 0;
        return (agentPos - point).normalized;
    }

    public static Vector3 Chase(Vector3 agentPos, Vector3 point)
    {
        agentPos.y = 0;
        point.y = 0;
        return -(agentPos - point).normalized;
    }
}
