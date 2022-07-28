using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISightCone : MonoBehaviour
{
    public bool enemyPositionKnownFromDamage = false;
    public float positionKnownTime = 5;
    public float timeLeftPositionKnown;

    public float distance = 10;
    public float height;
    public float angleHorizontal = 30;
    public float angleVertical = 20;

    public int scanFrequency = 30;
    public LayerMask layers;
    public LayerMask occlusionLayers;
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> inSightObjects = new List<GameObject>();

    Collider[] colliders = new Collider[50];
    float scanInterval;
    float scanTimer = 0;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
        timeLeftPositionKnown = positionKnownTime;
        
    }

    private void Update()
    {
        //Tank knows location of enemy if they are within line of sight (even if behind) when damage is taken (Called in Tank Health script)
        if (enemyPositionKnownFromDamage)
        {
            //If enough time has passed, stop knowing position of enemy
            timeLeftPositionKnown -= Time.deltaTime;
            if (timeLeftPositionKnown <= 0)
            {
                enemyPositionKnownFromDamage = false;
                timeLeftPositionKnown = positionKnownTime;
            }
        }
    }

    private void FixedUpdate()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }


    private void Scan()
    {
        //Remove all null objects
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i] == null)
            {
                objects.RemoveAt(i);
            }
        }

        for (int i = 0; i < inSightObjects.Count; i++)
        {
            if (inSightObjects[i] == null)
            {
                inSightObjects.RemoveAt(i);
            }
        }

        //For all objects in sight mesh
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i] != null)
            {
                //Spherecast towards object

                Vector3 origin = transform.position;
                Vector3 dest = objects[i].transform.position;
                Vector3 direction = dest - origin;
                origin.y += height / 2;

                RaycastHit hit;
                if (Physics.SphereCast(origin,0.5f ,direction, out hit, Mathf.Infinity))
                {
                    //Add object hit, to objects in sight. If targeted object not hit, remove it from objects in sight
                    if(hit.transform.gameObject != objects[i])
                    {
                        if (inSightObjects.Contains(objects[i]))
                        {
                            inSightObjects.Remove(objects[i]);
                        }
                        if(!inSightObjects.Contains(hit.transform.gameObject))
                        {
                            inSightObjects.Add(hit.transform.gameObject);
                        }
                        if (!objects.Contains(hit.transform.gameObject))
                        {
                            objects.Add(hit.transform.gameObject);
                        }
                    }
                    else
                    {
                        if (!inSightObjects.Contains(objects[i]))
                        {
                            inSightObjects.Add(objects[i]);
                        }
                    }
                }     
            }
        }
        //If position known form taking damage, Add enemy to seen objects
        if (enemyPositionKnownFromDamage)
        {
            if (enemyPositionKnownFromDamage)
            {
                if (gameObject.CompareTag("BTTank"))
                {

                    if (GameObject.FindGameObjectWithTag("PlannerTank") != null)
                    {
                        if (!inSightObjects.Contains(GameObject.FindGameObjectWithTag("PlannerTank")))
                        {
                            inSightObjects.Add(GameObject.FindGameObjectWithTag("PlannerTank"));
                        }
                        if (!objects.Contains(GameObject.FindGameObjectWithTag("PlannerTank")))
                        {
                            objects.Add(GameObject.FindGameObjectWithTag("PlannerTank"));
                        }
                    }
                }

                if (gameObject.CompareTag("PlannerTank"))
                {

                    if (GameObject.FindGameObjectWithTag("PlannerTank") != null)
                    {
                        if (!inSightObjects.Contains(GameObject.FindGameObjectWithTag("BTTank")))
                        {
                            inSightObjects.Add(GameObject.FindGameObjectWithTag("BTTank"));
                        }
                        if (!objects.Contains(GameObject.FindGameObjectWithTag("BTTank")))
                        {
                            objects.Add(GameObject.FindGameObjectWithTag("BTTank"));
                        }
                    }
                }

            }
        }
    }

    Mesh CreateSightMesh()
    {
        Mesh mesh = new Mesh();
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
        int numTriangles = 8;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 topCenter = Vector3.zero + Vector3.up * height;

        Vector3 bottomLeft = Quaternion.Euler(angleVertical, -angleHorizontal, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(angleVertical, angleHorizontal, 0) * Vector3.forward * distance;
        Vector3 topLeft = Quaternion.Euler(-angleVertical, -angleHorizontal, 0) * Vector3.forward * distance + Vector3.up * height;
        Vector3 topRight = Quaternion.Euler(-angleVertical, angleHorizontal, 0) * Vector3.forward * distance + Vector3.up * height;

        int vert = 0;

        //Left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        //Far side
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft;

        //Top
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight;

        //Bottom
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomLeft;

        for(int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateSightMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }

    //Add objects entering mesh to objects
    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.transform.gameObject))
        {
            objects.Add(other.transform.gameObject);
        }
    }

    //Remove objects leaving mesh from objects and inSightObjects
    private void OnTriggerExit(Collider other)
    {
        if(objects.Contains(other.transform.gameObject))
        {
            objects.Remove(other.transform.gameObject);
        }
        if (inSightObjects.Contains(other.transform.gameObject))
        {
            inSightObjects.Remove(other.transform.gameObject);
        }
    }
}
