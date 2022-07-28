using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeCoverBT : Node
{
    public AISightCone sightCone;

    public SeeCoverBT(Agent ownerAgent) : base(ownerAgent)
    {
        sightCone = ownerAgent.GetComponentInChildren<AISightCone>();
    }

    public override NODE_STATUS Update()
    {
        float minDistance = Mathf.Infinity;
        GameObject currentBestPosition = null;
            
        // Check if can see any cover objects
        for (int i = 0; i < sightCone.GetComponent<AISightCone>().inSightObjects.Count; i++)
        {
            if (sightCone.GetComponent<AISightCone>().inSightObjects[i].CompareTag("Cover"))
            {
                //Check that you are closer to cover than enemy
                if(Vector3.Distance(ownerAgent.lastKnownEnemyPosition, sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position) < Vector3.Distance(ownerAgent.transform.position, sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position))
                {
                    //Get postions around cover object that can act as cover
                    List<GameObject> coverNodes = new List<GameObject>();
                    foreach (Transform child in sightCone.GetComponent<AISightCone>().inSightObjects[i].transform)
                    {
                        if(child.tag == "CoverPoint")
                        {
                            coverNodes.Add(child.gameObject);
                        }
                    }
                    foreach(GameObject node in coverNodes)
                    {
                        //Check if position is out of sight of enemy
                        RaycastHit hit;
                        if (Physics.Raycast(node.transform.position, ownerAgent.lastKnownEnemyPosition, out hit, Mathf.Infinity))
                        {
                            if(hit.transform.tag != "PlannerTank" && hit.transform.gameObject != ownerAgent.gameObject)
                            {
                                //Check distance to cover. Closest cover is best
                                float distance = Vector3.Distance(ownerAgent.transform.position, node.transform.position);
                                if ( distance < minDistance)
                                {
                                    minDistance = distance;
                                    currentBestPosition = node;
                                }
                            }
                        }
                    }
                }             
            }
        }
        //Return best cover
        if(currentBestPosition != null)
        {
            ownerAgent.coverLocation = currentBestPosition;
            return NODE_STATUS.SUCCESS;
        }
        ownerAgent.coverLocation = null;
        return NODE_STATUS.FAILURE;
    }
}
