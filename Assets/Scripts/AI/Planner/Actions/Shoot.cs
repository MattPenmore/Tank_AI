using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : GAction
{
    public bool isShooting = false;
    public GameObject targ;
    TankSensors sensors;

    //private string m_FireButton;                // The input axis that is used for launching shells.
    public float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;

    float workDuration;
    public float costMultiplier;

    float forceDistanceMultiplier;
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;

    float shootTime;
    float rotationSpeed;

    private void Start()
    {
        actionName = "Shoot";
        sensors = gameObject.GetComponent<TankSensors>();
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        
        rotationSpeed = 2;
        forceDistanceMultiplier = 1f;
        // ... reset the fired flag and reset the launch force.
        m_CurrentLaunchForce = m_MinLaunchForce;
        // Change the clip to the charging clip and start it playing.
        //m_ShootingAudio.Play();
    }

    public override bool PrePerform()
    {
        isShooting = true;
        target = targ;
        shootTime = Time.time;
        return true;

    }

    public override bool PostPerform()
    {
        return true;
    }

    private void Update()
    {
        // Cost dependant on time will be shooting for
        //Set duration based on distance to enemy, charge speed and minimum launch force
        if (gameObject.GetComponent<TankSensors>().enemy != null)
        {
            workDuration = Mathf.Sqrt((Mathf.Max(gameObject.GetComponent<TankSensors>().enemyDistance * forceDistanceMultiplier, m_MinLaunchForce) - m_MinLaunchForce) / m_ChargeSpeed);
            cost = workDuration * costMultiplier;
        }
        else
        {
            cost = Mathf.Infinity;
        }

        if (isShooting)
        {
            m_AimSlider.gameObject.SetActive(true);
            //Always face target when shooting
            LookAtTarget();

            if (m_CurrentLaunchForce == m_MinLaunchForce)
            {
                shootTime = Time.time;
            }

            

            if (m_CurrentLaunchForce >= m_MaxLaunchForce)
            {
                // ... use the max force and launch the shell.
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();

            }

            // Otherwise, if the work duration has elapsed and the shell hasn't been launched yet...
            if (Time.time - shootTime >= workDuration)
            {
                // ... launch the shell.
                Fire();
            }
            // Otherwise, if the duration is not over...
            else
            {
                // Increment the launch force and update the slider.
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }

        }
        else
        {
            agent.updateRotation = true;
        }
    }


    void LookAtTarget()
    {
        if(gameObject.GetComponent<TankSensors>().enemy != null)
        {
            agent.updateRotation = false;
            Quaternion rotation = Quaternion.LookRotation(gameObject.GetComponent<TankSensors>().enemy.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
        }
    }

    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        InstantiateShell(m_CurrentLaunchForce);

        // Reset the launch force
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
        m_AimSlider.gameObject.SetActive(false);
        isShooting = false;

    }

    public void InstantiateShell(float launchForce)
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = launchForce * m_FireTransform.forward;
        
        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        sensors.shootTime = Time.time;
        sensors.m_ShotsUntilReload -= 1;
    }
}
