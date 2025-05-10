using UnityEngine;
using System.Collections.Generic;

public class Bots : MonoBehaviour
{
    [Header("Path Settings")]
    public List<Transform> waypoints = new List<Transform>();
    public float normalSpeed = 3f;
    public float chaseSpeed = 7f;
    public bool loop = true;
    public float delayAtWaypoint = 1f;

    [Header("Movement Settings")]
    public float acceleration = 5f;
    public float deceleration = 8f;

    [Header("Vision Settings")]
    [Range(0, 360)] public float fovAngle = 120f;
    public float visionRange = 10f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public float chaseDuration = 5f;

    private int currentWaypointIndex = 0;
    private float currentSpeed = 0f;
    private float delayTimer = 0f;
    private bool isWaiting = false;
    private Transform player;
    private bool isChasing = false;
    private Vector3 lastKnownPlayerPosition;
    private float chaseTimer = 0f;
    private float checkRate = 0.2f;
    private float nextCheckTime;
    public Animator RobotAnimator;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        nextCheckTime = Time.time;
        if (waypoints.Count > 0) transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 2f || player.gameObject.GetComponent<ThirdPersonCharacterController>().isDiying) {
            player.gameObject.GetComponent<ThirdPersonCharacterController>().Die();
            RobotAnimator.SetTrigger("Die");
            return;
        }
        if (waypoints.Count == 0) return;

        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkRate;
            CheckForPlayer();
        }

        if (isWaiting)
        {
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f) isWaiting = false;
            return;
        }

        float targetSpeed = isChasing ? chaseSpeed : normalSpeed;
        
        // Плавное изменение скорости
        if (currentSpeed < targetSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        }

        if (isChasing)
        {
            ChaseBehavior();
        }
        else
        {
            FollowPath();
        }
    }

    void CheckForPlayer()
    {
        if (player == null) return;
        if (player.gameObject.GetComponent<ThirdPersonCharacterController>().InBoxCanvas.activeSelf) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionRange) 
        {
            if (isChasing) chaseTimer -= checkRate;
            return;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= fovAngle / 2)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer))
            {
                StartChasing(player.position);
                return;
            }
        }

        if (isChasing)
        {
            chaseTimer -= checkRate;
        }
    }

    void StartChasing(Vector3 playerPosition)
    {
        isChasing = true;
        lastKnownPlayerPosition = playerPosition;
        chaseTimer = chaseDuration;
        // Мгновенное ускорение при обнаружении
        currentSpeed = Mathf.Max(currentSpeed, chaseSpeed * 0.8f);
    }

    void ChaseBehavior()
    {
        if (chaseTimer <= 0)
        {
            isChasing = false;
            return;
        }

        if (player != null && CanSeePlayerDirectly())
        {
            lastKnownPlayerPosition = player.position;
            currentSpeed = Mathf.MoveTowards(currentSpeed, chaseSpeed, acceleration * Time.deltaTime);
        }

        MoveToTarget(lastKnownPlayerPosition);

        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 0.5f)
        {
            chaseTimer -= Time.deltaTime;
        }
    }

    bool CanSeePlayerDirectly()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer <= fovAngle / 2 && 
               distanceToPlayer <= visionRange && 
               !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);
    }

    void MoveToTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        
        if (target - transform.position != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    void FollowPath()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveToTarget(targetWaypoint.position);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartWaiting();
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            if (!loop && currentWaypointIndex == 0) enabled = false;
        }
    }

    void StartWaiting()
    {
        isWaiting = true;
        delayTimer = delayAtWaypoint;
        currentSpeed = 0f;
    }

    private void OnDrawGizmos()
    {
        // Визуализация поля зрения
        Gizmos.color = isChasing ? new Color(1, 0, 0, 0.3f) : new Color(0, 0, 1, 0.3f);
        Vector3 leftBound = Quaternion.Euler(0, -fovAngle / 2, 0) * transform.forward * visionRange;
        Vector3 rightBound = Quaternion.Euler(0, fovAngle / 2, 0) * transform.forward * visionRange;
        
        Gizmos.DrawRay(transform.position, leftBound);
        Gizmos.DrawRay(transform.position, rightBound);
        Gizmos.DrawLine(transform.position + leftBound, transform.position + rightBound);

        // Визуализация пути
        if (waypoints.Count < 2) return;

        Gizmos.color = isChasing ? new Color(1, 0, 0, 0.5f) : new Color(1, 1, 0, 0.5f);
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (loop && waypoints.Count > 1 && waypoints[0] != null && waypoints[waypoints.Count - 1] != null)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }

        if (isChasing)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(lastKnownPlayerPosition, 0.5f);
            Gizmos.DrawLine(transform.position, lastKnownPlayerPosition);
        }
    }
}