using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public float normalSpeed = 3f;
    public float runSpeed = 5f;
    public float detectionRange = 8f;
    public int maxHP = 30;

    public float attackRate = 3f;
    public float attackRange = 2f;
    public int attackDamage = 5;

    public GameObject meatPrefab;

    private Transform player;
    private int currentHP;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float nextAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");

        SetRandomDestination();
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChangeSpeed(runSpeed);
            animator.SetTrigger("Run");

            navMeshAgent.SetDestination(player.position);

            if (distanceToPlayer<= attackRange)
            {
                TryAttack();
            }
        }
        else
        {
            ChangeSpeed(normalSpeed);
            animator.SetTrigger("Walk");

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                SetRandomDestination();
            }

        }
    }
    private void ChangeSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }
    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);
    }
    private void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
            animator.SetTrigger("Attack");
        }
    }
    public void Attack()
    {
        Player player = GetComponent<Player>();
        if (player != null)
        {
            //player.Damaged(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }
    private IEnumerator DieCoroutine()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SpawnMeat();
    }
    private void SpawnMeat()
    {
        Vector3 meatSpawnPosition = transform.position + Vector3.up;
        Quaternion meatSpawnRotation = Quaternion.identity;

        GameObject meatObject = Instantiate(meatPrefab, meatSpawnPosition, Quaternion.identity);
    }
}
