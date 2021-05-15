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
    [SerializeField]
    float changeDirThreshold = 0.1f, movementSpeed = 10f;
    Vector3 maxInterest;


    void Start()
    {        
        interestMap = new Vector3[resolution];
        dangerMap = new float[resolution];
        
        InvokeRepeating("UpdatePath", 0f, 0.2f);
    }
    private void UpdatePath()
    {
        maxInterest = FindMaxInterest();   
    }
    
    
    private void FixedUpdate()
    {
        rb.velocity = maxInterest*movementSpeed;
    }

   
    Vector3 FindMaxInterest()
    {
        Vector3 moveV = (goal.position - transform.position).normalized;
        //Vector3 moveV = transform.forward;
        System.Array.Clear(interestMap, 0, interestMap.Length);
        System.Array.Clear(dangerMap, 0, dangerMap.Length);
        for (int i = 0; i < resolution; i++)
        {
            Vector3 dir = Quaternion.Euler(0, (360f / resolution) * i, 0) * transform.forward;
            RaycastHit hit;
            
            if (Physics.SphereCast(transform.position, 1f, dir, out hit, 10f))
            {
                
                if (hit.transform.CompareTag("Avoid"))
                {
                    dangerMap[i] = 1 / hit.distance;                    
                }

            }
            float d = Vector3.Dot(moveV, dir);//find difference between desired vector and proposed movement
            d = (d + 1) / 2;//we normalize the values between 0:1

            //we do this because the opposite vector of the desired direction is the worst case so its zero and as we get closer
            //to the desired direction our values get closer to one
            
            interestMap[i] = d * dir;
           
            Debug.DrawRay(transform.position, interestMap[i], Color.white);
           
        }
        //Masking
        List<int> index = new List<int>();
        index.Clear();
        float minDanger = float.MaxValue;
        for (int i = 0; i < resolution; i++)
        {
            
            if (dangerMap[i] <= minDanger)
            {
                minDanger = dangerMap[i];
            }
        }

        for (int i = 0; i < resolution; i++)
        {
            if (dangerMap[i] == minDanger)
            {
                index.Add(i);

            }
        }

        Vector3 maxInterest = new Vector3(0, 0, 0);
        for (int i = 0; i < index.Count; i++)
        {
            if (interestMap[index[i]].magnitude >= maxInterest.magnitude + changeDirThreshold)
            {
                maxInterest = interestMap[index[i]];               
            }
            
        }

        Debug.DrawRay(transform.position, maxInterest, Color.green);
        maxInterest = maxInterest.normalized;
        this.maxInterest = maxInterest;
        return maxInterest;
    }

}
