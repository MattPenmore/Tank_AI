using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : GAction
{
    public bool m_IsReloading = false;
    public GameObject targ;
    TankSensors sensors;
    public float m_ReloadTime = 5f;             // Time it takes to reload
    public float costMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        actionName = "Reload";
        sensors = gameObject.GetComponent<TankSensors>();
    }

    public override bool PrePerform()
    {
        m_IsReloading = true;
        target = targ;
        startTime = Time.time;
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }



    // Update is called once per frame
    void Update()
    {
        cost = sensors.m_ShotsUntilReload * costMultiplier;
        if(m_IsReloading)
        {
            if (Time.time - startTime > m_ReloadTime)
            {
                m_IsReloading = false;
                sensors.m_ShotsUntilReload = sensors.m_ShotsBeforeReload;
            }
        }
    }
}
