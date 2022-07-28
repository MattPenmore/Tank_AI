using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerTank : GAgent
{
    TankSensors sensors;
    public float shouldEscapeHealthMultiplier;
    public float shouldEscapeMultiplier;
    public int shouldEscapeAddition;
    public float shouldEscapeDistanceMultiplier;
    public float surviveMultiplier;

    public float killEnemyBase;
    public float killEnemyMultiplier;
    SubGoal findEnemy;
    SubGoal survive;
    SubGoal escape;
    SubGoal killEnemy;
    SubGoal prepare;

    // Start is called before the first frame update
    new void Start()
    {
        //Create all tank goals
        base.Start();
        sensors = gameObject.GetComponent<TankSensors>();
        findEnemy = new SubGoal("FindEnemy", 1, false);
        survive = new SubGoal("Survive", 1, false);
        escape = new SubGoal("Escape", 1, false);
        killEnemy = new SubGoal("KillEnemy", 1, false);
        prepare = new SubGoal("Prepare", 1, false);
        goals.Add(findEnemy, 1);
        goals.Add(survive, 1);
        goals.Add(killEnemy, 1);
        goals.Add(escape, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float healthDif = sensors.KnownEnemyHealth - sensors.health;

        //Set importance of goals based on values from Tank Sensor
        goals[escape] = Mathf.RoundToInt((healthDif - shouldEscapeHealthMultiplier * sensors.health) * shouldEscapeMultiplier) + shouldEscapeAddition - Mathf.RoundToInt(sensors.enemyDistance * shouldEscapeDistanceMultiplier);

        goals[killEnemy] = Mathf.RoundToInt((100 - sensors.KnownEnemyHealth + killEnemyBase) * killEnemyMultiplier);

        if(sensors.seeEnemy)
        {
            goals[findEnemy] = 0;
        }
        else
        {
            goals[findEnemy] = 30;
            goals[survive] = 0;
        }
        if(sensors.shotAt)
        {
            goals[survive] = 100;
        }
        else
        {
            goals[survive] = 100 - Mathf.RoundToInt(sensors.enemyDistance * surviveMultiplier);
        }

        goals[prepare] = (sensors.m_ShotsBeforeReload - sensors.m_ShotsUntilReload) / sensors.m_ShotsBeforeReload * 100;
    }
        
    
}
