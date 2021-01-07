using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

//Used to apply the actual values for the the movements of the agent after calculations and make him move
public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
       
    [SerializeField]
    Transform goal;
    [SerializeField]
    int resolution = 24;
    Vector3[] interestMap;
    float[] dangerMap;
   
    private NavMeshPath path;
    
    // Start is called before the first frame update
    void Start()
    {
        interestMap = new Vector3[resolution];
        dangerMap = new float[resolution];
       
        path = new NavMeshPath();
    }

    // Update is called once per frame
    
    private void FixedUpdate()
    {
        Vector3 moveV = (goal.position-transform.position).normalized;
        for(int i = 0; i < resolution; i++)
        {
            Vector3 dir = Quaternion.Euler(0, (360f/resolution) * i, 0)*transform.forward;            
            float d = Vector3.Dot(moveV, dir);//find difference between desired vector and proposed movement
            d = (d + 1) / 2;//we normalize the values between 0:1
            
            //we do this because the opposite vector of the desired direction is the worst case so its zero and as we get closer
            //to the desired direction our values get closer to one
            //Debug.DrawRay(transform.position, dir * d);
            interestMap[i] = d*dir;
           
            //Debug.DrawRay(transform.position, interestMap[i], Color.white);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position,2f,dir, out hit, 2f))
            {
                if (hit.transform.CompareTag("Avoid"))
                {
                    dangerMap[i] = 1 / hit.distance;
                }
            }
            else
            {
                dangerMap[i] = 0;
            }

        }
        
        //Masking
        List<int> index = new List<int>();
        index.Clear();
        float minDanger = float.MaxValue;
        for(int i = 0; i < resolution; i++)
        {
            Debug.DrawRay(transform.position, interestMap[i], Color.green); 
            if (dangerMap[i] <= minDanger)
            {
                minDanger = dangerMap[i];
                
            }            
        }
        for(int i = 0; i < resolution; i++)
        {
            if (dangerMap[i] == minDanger)
            {
                index.Add(i);         
            }
        }
        
        Vector3 maxInterest = Vector3.zero;
        for(int i = 0; i < index.Count; i++)
        {
            if (interestMap[index[i]].magnitude >= maxInterest.magnitude)
            {
                maxInterest = interestMap[index[i]];               
            }
            Debug.DrawRay(transform.position, interestMap[index[i]], Color.red);
            
        }
        Debug.DrawRay(transform.position, maxInterest,Color.green);
        
        rb.velocity = maxInterest * 2;
        
            
    }
    
    //void OnDrawGizmos()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    Vector3 moveV = (goal.position - transform.position).normalized;
        
        
    //    for (int i = 0; i < 8; i++)
    //    {

    //        Vector3 dir = Quaternion.Euler(0, 45f*i, 0) * transform.forward;
    //        dir = dir.normalized;
    //        float d = Vector3.Dot(moveV, dir);//find difference between desired vector and proposed movement
    //        d = (d + 1) / 2;//we normalize the values between 0:1
    //        //we do this because the opposite vector of the desired direction is the worst case so its zero and as we get closer
    //        //to the desired direction our values get closer to one
    //        Gizmos.DrawRay(transform.position, dir * d);
            
    //    }




    //}
}
