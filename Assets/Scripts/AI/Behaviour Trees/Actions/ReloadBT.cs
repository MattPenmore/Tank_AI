using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBT : Node
{
    bool m_IsReloading;
    public float m_ReloadTime = 5f;             // Time it takes to reload

    float startTime;
    public ReloadBT(Agent ownerAgent) : base(ownerAgent)
    {
        m_IsReloading = false;
    }

    public override NODE_STATUS Update()
    {
        //Start Reloading
        if(!m_IsReloading)
        {
            m_IsReloading = true;
            startTime = Time.time;
        }
        
        //If finished reloading, set ammo to full
        if (Time.time - startTime > m_ReloadTime)
        {
            m_IsReloading = false;
            ownerAgent.GetComponent<Agent>().m_ShotsUntilReload = ownerAgent.GetComponent<Agent>().m_ShotsBeforeReload;
        }

        if (m_IsReloading)
        {
            //Tell tree that it is still reloading
            return NODE_STATUS.RUNNING;
        }
        else
        {
            return NODE_STATUS.SUCCESS;
        }
    }
}
