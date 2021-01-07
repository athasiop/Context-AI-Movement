using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleMove 
{
    public Vector3 direction;
    public float weight;
    
    public PossibleMove(Vector3 direction,float weight)
    {
        this.direction = direction;
        this.weight = weight;       
    }
}
