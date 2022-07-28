using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAtBT : Node
{
    public GameObject sightCone;
    public float dotLimit = -0.7f;

    public ShotAtBT(Agent ownerAgent) : base(ownerAgent)
    {
        sightCone = ownerAgent.sightCone;
    }

    public override NODE_STATUS Update()
    {
        //Check if tank can see a Bullet
        for (int i = 0; i < sightCone.GetComponent<AISightCone>().inSightObjects.Count; i++)
        {
            if (sightCone.GetComponent<AISightCone>().inSightObjects[i].CompareTag("Bullet") && sightCone.GetComponent<AISightCone>().inSightObjects[i] != null)
            {
                //Check direction bullet is travelling
                Vector3 directionOfBullet = (sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position - sightCone.transform.position).normalized;
                Vector3 bulletTravelDirection = sightCone.GetComponent<AISightCone>().inSightObjects[i].GetComponent<Rigidbody>().velocity.normalized;
                float dot = Vector3.Dot(directionOfBullet, bulletTravelDirection);

                //If travelling towards tank, return success
                if(dot < dotLimit)
                {
                    ownerAgent.shotAtDirection = directionOfBullet;
                    return NODE_STATUS.SUCCESS;
                }               
            }
        }
        return NODE_STATUS.FAILURE;
    }
}
