using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public float shootRange = 10;
    public float enemyTooCloseRange = 20;
    public GameObject enemyTarget;
    public GameObject coverLocation;
    public ShootingUpdateBT shootUpdate;
    public float shootTime = 0;
    public float evasionTime;
    public float evasionDuration = 2;
    public int lowHealthThreshhold = 30;
    public int lastKnownEnemyHealth = 100;
    public Vector3 lastKnownEnemyPosition;
    public float distanceToEnemy;
    public float EnemyPositionKnownTime = float.NaN;

    public Vector3 shotAtDirection;

    public static List<Agent> agentList = new List<Agent>();

    public NavMeshAgent navComp { get; private set; }
    public BTTank behaviourTree;

    public GameObject sightCone;
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.

    public float m_TimeBetweenShots = 1.0f;     // The minimum time between shots

    public int m_ShotsBeforeReload = 6;         // Maximum shots that can be fired before reloading
    public int m_ShotsUntilReload = 6;
    public bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        //Create tree and set navmesh agent
        agentList.Add(this);
        navComp = gameObject.GetComponent<NavMeshAgent>();
        behaviourTree = new BTTank(this);
    }

    // Update is called once per frame
    void Update()
    {
        behaviourTree.Update();
    }

    public void InstantiateShell(float launchForce)
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = launchForce * m_FireTransform.forward;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_ShotsUntilReload -= 1;
    }

    public void Reset()
    {
        //Reset all values to what they start as
        enemyTarget = null;
        coverLocation = null;
        shootTime = 0;
        evasionTime = 0;
        lastKnownEnemyPosition = Vector3.zero;
        shotAtDirection = Vector3.zero;
        EnemyPositionKnownTime = float.NaN;
        m_ShotsUntilReload = m_ShotsBeforeReload;
        canShoot = false;
        sightCone.GetComponent<AISightCone>().enemyPositionKnownFromDamage = false;
        sightCone.GetComponent<AISightCone>().timeLeftPositionKnown = 0;
        sightCone.GetComponent<AISightCone>().inSightObjects.Clear();
        sightCone.GetComponent<AISightCone>().objects.Clear();
        behaviourTree = new BTTank(this);

        shootUpdate.m_CurrentLaunchForce = shootUpdate.m_MinLaunchForce;
        shootUpdate.isShooting = false;
        shootUpdate.m_AimSlider.gameObject.SetActive(false);
        navComp.updateRotation = true;
    }
}
