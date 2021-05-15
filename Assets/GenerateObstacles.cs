using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : MonoBehaviour
{
    public GameObject obs;
    float timer = 0;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Instantiate(obs,new Vector3(UnityEngine.Random.Range(-2.5f,2.5f),transform.position.y,transform.position.z),Quaternion.identity);
            timer = 2f;
        }
    }
}
