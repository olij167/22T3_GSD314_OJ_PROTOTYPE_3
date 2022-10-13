using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolToRandomWayPoint : MonoBehaviour
{
    public NavMeshAgent agent;
    public NPCBrain npcBrain;

    private bool speakingToPlayer;

    [SerializeField] private Vector3 waypoint;

    float wayPointTimer, timerReset;

    private GameObject player;
    void Start()
    {
        timerReset = Random.Range(5f, 10f);
        wayPointTimer = timerReset;
        player = GameObject.FindGameObjectWithTag("Player");
        SetWaypoint();
    }

    void Update()
    {
        if (npcBrain.speakingToPlayer)
        {
            speakingToPlayer = true;
        }
        else
        {
            speakingToPlayer = false;
        }

        if (!speakingToPlayer)
        {
            wayPointTimer -= Time.deltaTime;
            if (Vector3.Distance(agent.transform.position, waypoint) <= 5f || wayPointTimer <= 0f)
            {
                SetWaypoint();
                wayPointTimer = timerReset;
            }

            agent.SetDestination(waypoint);
        }
        else
        {
            agent.isStopped = true;
            agent.transform.LookAt(player.transform);
        }

    }

    public void SetWaypoint()
    {
        waypoint = GenerateRandomWayPoint();
    }

    public Vector3 GenerateRandomWayPoint()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int maxIndices = navMeshData.indices.Length - 3;

        // pick the first indice of a random triangle in the nav mesh
        int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);

        // spawn on verticies
        Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];

        Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];

        // eliminate points that share a similar X or Z position to stop spawining in square grid line formations
        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
        {
            point = GenerateRandomWayPoint(); // re-roll a position - I'm not happy with this recursion it could be better
        }
        else
        {
            // select a random point on it
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, UnityEngine.Random.Range(0.05f, 0.95f));
        }

        return point;
    }
}
