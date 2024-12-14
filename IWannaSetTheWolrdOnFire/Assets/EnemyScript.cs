using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState = EnemyState.Patrol;

    [SerializeField] private Animator animator;
    [SerializeField] private int enemyHP;
    [SerializeField] private bool isAlive = true;
    [SerializeField] private int enemyDMG = 5;

    [Header("References")]
    public NavMeshAgent agent;
    public GameObject player;
    PlayerMovement PMScript;

    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;

    [Header("Chase Settings")]
    public float chaseRange = 15f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;

    private float lastAttackTime = 0f;
    private Vector3 patrolTarget;
    private bool isPatrolTargetSet = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        PMScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isAlive&&PMScript.canMove)
        {
            switch (currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
            }

            CheckStateTransition();
        }
        if (enemyHP <= 0)
        {
            isAlive = false;
            animator.SetTrigger("Death");
            Destroy(gameObject, 3f);
        }
    }

    private void Patrol()
    {
        animator.SetTrigger("Walk");
        if (!isPatrolTargetSet)
        {
            patrolTarget = GetRandomPatrolPoint();
            agent.SetDestination(patrolTarget);
            isPatrolTargetSet = true;
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(WaitBeforeNextPatrol());
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    private void Attack()
    {
        agent.ResetPath(); // Stop moving
        FacePlayer();
        animator.SetTrigger("Attack");

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemy attacks player!");
            lastAttackTime = Time.time;
            // Add attack logic here (e.g., reduce player's health)
            PMScript.DecreseHp(enemyDMG);
            
        }
    }

    private void CheckStateTransition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private IEnumerator WaitBeforeNextPatrol()
    {
        isPatrolTargetSet = false;
        yield return new WaitForSeconds(patrolWaitTime);
    }

    private Vector3 GetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        return hit.position;
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void DecraseEnemyHP()
    {
        enemyHP -= 5;
    }

}
