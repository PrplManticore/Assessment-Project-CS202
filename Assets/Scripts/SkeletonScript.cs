using UnityEngine;
using UnityEngine.AI;

public class SkeletonScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float attackRange = 30.0f, sightRange = 60.0f;
    // [SerializeField] float contact = 1.0f;
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer, groundLayer;
    [SerializeField] float patrolRange = 100.0f;
    [SerializeField] GameObject enemySlash;
    [SerializeField] Transform slashTransform;
    Vector3 walkPoint;

    bool playerInSightRange, playerInAttackRange;
    bool basicAttackInContactRange;
    bool walkPointSet;
    bool alreadyAttacking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void ChasePlayer()
    {
        if (playerInSightRange)
        {
            agent.SetDestination(player.position);
        }
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stop moving to attack
        transform.LookAt(player);

        if(!alreadyAttacking)
        {
            Instantiate(enemySlash, slashTransform.position, slashTransform.rotation);

            alreadyAttacking = true;
            Invoke(nameof(ResetAttack), 3.0f);
        }
    }

    void ResetAttack()
    {
        alreadyAttacking = false;
    }

    void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        
        Vector3 distanceToWalk = transform.position - walkPoint;
        if (distanceToWalk.magnitude < 1.0f) walkPointSet = false;
        
        if (walkPointSet)
        {
            Debug.Log(walkPoint);
            agent.SetDestination(walkPoint);
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.CheckSphere(walkPoint, 2.0f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
