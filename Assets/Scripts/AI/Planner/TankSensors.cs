using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankSensors : MonoBehaviour
{
    public PlannerTank agent;
    public Shoot shootAction;
    public AISightCone sightCone;
    public int KnownEnemyHealth;
    public GameObject enemy;
    bool coverSeen;
    public GameObject coverLocation;
    public float coverDistance;
    public bool seeEnemy;
    public bool shotAt;
    public float seeEnemyTime;
    float seenEnemyRecentlyTime = 5f; 
    public Vector3 knownEnemyPosition;
    public float enemyDistance;

    public Vector3 shotAtDirection;
    public float dotLimit = -0.7f;

    public float shootRange;
    public int health;

    public float shootTime;
    float m_TimeBetweenShots = 1.0f;     // The minimum time between shots

    public int m_ShotsBeforeReload = 6;         // Maximum shots that can be fired before reloading
    public int m_ShotsUntilReload = 6;

    private void Update()
    {
        EnemySeen();
        EnemyDistance();
        SeenByEnemy();
        LastKnownEnemyHealth();
        SeeCover();
        shotAt = ShotAt();
        health = Mathf.RoundToInt(transform.GetComponent<TankHealth>().m_CurrentHealth);

        //Set world states based on sensors
        if(seeEnemy)
        {
            GWorld.world.SetState("SeeEnemy", 1);
            GWorld.world.SetState("EnemyHealth", KnownEnemyHealth);
            if (enemyDistance < shootRange)
            {
                GWorld.world.SetState("InRange", 1);
            }
            else
            {
                GWorld.world.SetState("InRange", 0);
            }

            if(shotAt)
            {
                GWorld.world.SetState("ShotAt", 1);
            }
            else
            {
                GWorld.world.SetState("ShotAt", 0);
            }


            if(SeenByEnemy())
            {
                GWorld.world.SetState("SeenByEnemy", 1);
            }
            else
            {
                GWorld.world.SetState("SeenByEnemy", 0);
            }
        }
        else
        {
            GWorld.world.SetState("SeeEnemy", 0);
            GWorld.world.SetState("InRange", 0);
            GWorld.world.SetState("ShotAt", 0);
            GWorld.world.SetState("SeenByEnemy", 0);
        }
        GWorld.world.SetState("Health", health);

        if (coverSeen)
        {
            GWorld.world.SetState("CoverSeen", 1);
        }
        else
        {
            GWorld.world.SetState("CoverSeen", 0);
        }

        if(Time.time - seenEnemyRecentlyTime < seeEnemyTime && seeEnemyTime != 0)
        {
            GWorld.world.SetState("SeenEnemyRecently", 1);
        }
        else
        {
            GWorld.world.SetState("SeenEnemyRecently", 0);
        }

        if(m_ShotsUntilReload <= 0)
        {
            GWorld.world.SetState("AmmoEmpty", 1);
            GWorld.world.SetState("AmmoFull", 0);
        }
        else
        {
            GWorld.world.SetState("AmmoEmpty", 0);
            if(m_ShotsUntilReload == m_ShotsBeforeReload)
            {
                GWorld.world.SetState("AmmoFull", 1);
            }
            else
            {
                GWorld.world.SetState("AmmoFull", 0);
            }
        }

        if(Time.time - m_TimeBetweenShots > shootTime && gameObject.GetComponent<Shoot>().isShooting == false)
        {
            GWorld.world.SetState("CanShoot", 1);

        }
        else
        {
            GWorld.world.SetState("CanShoot", 0);
        }

        if(gameObject.GetComponent<Reload>().m_IsReloading == true)
        {
            GWorld.world.SetState("IsReloading", 1);
        }
        else
        {
            GWorld.world.SetState("IsReloading", 0);
        }

    }

    //Check if enemy can see this tank
    public bool SeenByEnemy()
    {
        //Can only tell if can see the enemy
        if(seeEnemy)
        {
            AISightCone enemySight = enemy.GetComponentInChildren<AISightCone>();
            if (enemySight.inSightObjects.Contains(gameObject))
            {
                return true;
            }
        }
        return false;
    }

    //If can see the enemy, update the known enemy health
    public float LastKnownEnemyHealth()
    {
        if(seeEnemy)
        {
            KnownEnemyHealth = Mathf.RoundToInt(enemy.GetComponent<TankHealth>().m_CurrentHealth);
        }
        return KnownEnemyHealth;
    }

    //Check if Tank can see enemy
    public bool EnemySeen()
    {
        for (int i = 0; i < sightCone.inSightObjects.Count; i++)
        {
            if(sightCone.inSightObjects[i] != null)
            {
                if (sightCone.inSightObjects[i].CompareTag("BTTank"))
                {
                    seeEnemy = true;
                    enemy = sightCone.inSightObjects[i];
                    seeEnemyTime = Time.time;
                    knownEnemyPosition = enemy.transform.position;
                    return true;
                }
            }

        }
        seeEnemy = false;
        return false;
    }

    //If can see enemy, get distance to them, otherwise make it infinite
    public float EnemyDistance()
    {
        if (seeEnemy)
        {
            enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);          
        }
        return enemyDistance;
    }

    public bool SeeCover()
    {
        float minDistance = Mathf.Infinity;
        GameObject currentBestPosition = null;

        for (int i = 0; i < sightCone.GetComponent<AISightCone>().inSightObjects.Count; i++)
        {
            if(sightCone.GetComponent<AISightCone>().inSightObjects[i] != null)
            {
                if (sightCone.GetComponent<AISightCone>().inSightObjects[i].CompareTag("Cover"))
                {
                    //Check that you are closer to cover than enemy
                    if (Vector3.Distance(knownEnemyPosition, sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position) < Vector3.Distance(transform.position, sightCone.GetComponent<AISightCone>().inSightObjects[i].transform.position))
                    {
                        //Get postions around cover object that can act as cover
                        List<GameObject> coverNodes = new List<GameObject>();
                        foreach (Transform child in sightCone.GetComponent<AISightCone>().inSightObjects[i].transform)
                        {
                            if (child.tag == "CoverPoint")
                            {
                                coverNodes.Add(child.gameObject);
                            }
                        }
                        foreach (GameObject node in coverNodes)
                        {
                            //Check if position is out of sight of enemy
                            RaycastHit hit;
                            if (Physics.Raycast(node.transform.position, knownEnemyPosition - node.transform.position, out hit, Mathf.Infinity))
                            {
                                if (hit.transform.tag != "BTTank" && hit.transform.gameObject != gameObject)
                                {
                                    //Check distance to cover. Closest cover is best
                                    float distance = Vector3.Distance(transform.position, node.transform.position);
                                    if (distance < minDistance)
                                    {
                                        minDistance = distance;
                                        currentBestPosition = node;
                                        coverDistance = distance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //Return best cover
        if (currentBestPosition != null)
        {
            coverLocation = currentBestPosition;
            coverSeen = true;
            return true;
        }
        

        coverSeen = false;
        coverDistance = Mathf.Infinity;
        return false;
    }

    public bool ShotAt()
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
                shotAtDirection = directionOfBullet;

                //If travelling towards tank, return success
                if (dot < dotLimit)
                {                   
                    return true;
                }
            }
        }
        return false;
    }

    public void Reset()
    {
        //Reset all values to what they start at
        sightCone.GetComponent<AISightCone>().enemyPositionKnownFromDamage = false;
        sightCone.GetComponent<AISightCone>().timeLeftPositionKnown = 0;
        sightCone.GetComponent<AISightCone>().inSightObjects.Clear();
        sightCone.GetComponent<AISightCone>().objects.Clear();
        KnownEnemyHealth = 100;
        GWorld.world.SetState("EnemyHealth", KnownEnemyHealth);
        enemy = null;
        coverSeen = false;
        coverLocation = null;
        coverDistance = Mathf.Infinity;
        seeEnemy = false;
        shotAt = false;
        seeEnemyTime = 0;
        knownEnemyPosition = Vector3.zero;
        enemyDistance = Mathf.Infinity;
        shotAtDirection = Vector3.zero;
        health = 100;
        shootTime = 0;
        m_ShotsUntilReload = m_ShotsBeforeReload;

        if(agent != null)
        {
            if(agent.currentAction)
            {
                agent.currentAction.running = false;
            }
            agent.currentAction = null;
            agent.actionQueue = null;
            agent.planner = null;
        }

        shootAction.isShooting = false;
        shootAction.m_AimSlider.gameObject.SetActive(false);
        shootAction.m_CurrentLaunchForce = shootAction.m_MinLaunchForce;
        gameObject.GetComponent<NavMeshAgent>().updateRotation = true;
    }
}
