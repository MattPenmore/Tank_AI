using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ShootingUpdateBT : MonoBehaviour
{
    Agent ownerAgent;

    public float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.

    private float startTime = 0;
    float workDuration;                         // How long the bullet needs to be fired ofr to get correct force
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

    float rotationSpeed;                        // Speed Tank rotates to look at enemy

    public bool isShooting;                     // Whether the tank is currently shooting

    // Start is called before the first frame update
    void Start()
    {
        ownerAgent = gameObject.GetComponent<Agent>();
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        rotationSpeed = 2;
        forceDistanceMultiplier = 1f;
        m_CurrentLaunchForce = m_MinLaunchForce;

    }

    // Update is called once per frame
    void Update()
    {
        if(isShooting)
        {
            if (m_CurrentLaunchForce == m_MinLaunchForce)
            {
                //Set Time when shooting started
                startTime = Time.time;
                // Change the clip to the charging clip and start it playing.
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }

            //Always face target when shooting
            LookAtTarget();

            //Set duration based on distance to enemy, charge speed and minimum launch force
            workDuration = Mathf.Sqrt((Mathf.Max(ownerAgent.distanceToEnemy * forceDistanceMultiplier, m_MinLaunchForce) - m_MinLaunchForce) / m_ChargeSpeed);


            if (m_CurrentLaunchForce >= m_MaxLaunchForce)
            {
                // ... use the max force and launch the shell.
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
                isShooting = false;

            }

            // Otherwise, if the work duration has elapsed and the shell hasn't been launched yet...
            if (Time.time - startTime >= workDuration)
            {
                // ... launch the shell.
                Fire();
                isShooting = false;
            }
            // Otherwise, if the duration is not over...
            else
            {
                // Increment the launch force and update the slider.
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

                m_AimSlider.value = m_CurrentLaunchForce;
            }
        }

    }

    private void Fire()
    {
        ownerAgent.navComp.updateRotation = true;
        // Create an instance of the shell and store a reference to it's rigidbody.
        ownerAgent.GetComponent<Agent>().InstantiateShell(m_CurrentLaunchForce);
        ownerAgent.shootTime = Time.time;
        // Reset the launch force.
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }

    void LookAtTarget()
    {
        ownerAgent.navComp.updateRotation = false;
        Quaternion rotation = Quaternion.LookRotation(ownerAgent.enemyTarget.transform.position - ownerAgent.transform.position);
        ownerAgent.transform.rotation = Quaternion.Slerp(ownerAgent.transform.rotation, rotation, rotationSpeed);
    }
}
