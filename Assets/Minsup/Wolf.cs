using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public enum WolfState
    {
        Walking,
        Running,
        Attacking
    }

    public float normalSpeed = 2f; // ������ �⺻ �ӵ�
    public float runSpeed = 4f; // ������ ���� �ӵ�
    public float detectionRange = 8f; // �÷��̾ �����ϴ� �Ÿ�
    public int maxHP = 30; // ������ �ִ� ü��
    public int attackDamage = 5; // ������ ���ݷ�
    public float attackInterval = 3f; // ������ ���� ����
    public float attackRange = 2.7f;

    public GameObject meatPrefab;

    private Transform player; // �÷��̾��� ��ġ
    private int currentHP; // ������ ���� ü��
    private NavMeshAgent navMeshAgent;
    private NavMeshPath navMeshPath;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private WolfState currentState = WolfState.Walking;

    private Vector3 destinationPosition;
    public float changeDestinationInterval = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾ �±׷� ã�Ƽ� �Ҵ�
        currentHP = maxHP; // ���� ü���� �ִ� ü������ ����

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Walk");

        // ������ �ʱ� ������ ����
        InvokeRepeating("SetRandomDestination", changeDestinationInterval, changeDestinationInterval);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (CanAttack())
            {
                // �÷��̾ ���� �Ÿ� �ȿ� �ְ�, ���� ������ ���¶�� ���� ���·� ����
                currentState = WolfState.Attacking;
            }
            else
            {
                // �� �ܿ��� �پ�� ���·� ����
                currentState = WolfState.Running;
            }
        }
        else
        {
            // �÷��̾ ���� �Ÿ� �ۿ� ������ Idle ���·� ����
            currentState = WolfState.Walking;
        }

        // ���¿� ���� ������ �����մϴ�.
        switch (currentState)
        {
            case WolfState.Walking:
                ChangeSpeed(normalSpeed);
                animator.SetTrigger("Walk");
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
                {
                    SetRandomDestination();
                }
                break;

            case WolfState.Running:
                ChangeSpeed(runSpeed);
                animator.SetTrigger("Run");
                navMeshAgent.SetDestination(player.position);
                break;

            case WolfState.Attacking:
                MoveToPlayerAndAttack();
                break;
        }
    }

    private void MoveToPlayerAndAttack()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            navMeshAgent.SetDestination(transform.position);
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            //Vector3 attackPosition = player.position - (directionToPlayer * attackRange);
            //navMeshAgent.SetDestination(attackPosition);
            // ��ֹ� ȸ�� ���� �߰�
            navMeshAgent.CalculatePath(player.position, navMeshPath);
            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                // ��ֹ��� ���� ��ΰ� ���� ���, �÷��̾� ��ġ�� �̵�
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                // ��ֹ��� �־ ��ΰ� �����ִ� ���, ������ ��ġ�� �̵�
                SetRandomDestination();
            }
        }
        else if (distanceToPlayer > detectionRange)
        {
            SetRandomDestination();
        }
        else { SetRandomDestination(); }
    }

    private void ChangeSpeed(float speed)
    {
        // NavMeshAgent�� �ӵ��� �����ϴ� �޼ҵ�
        navMeshAgent.speed = speed;
    }

    private void SetRandomDestination()
    {
        // ������ ��ġ�� �̵��� ������ ����
        float randomDistance = Random.Range(10f, 15f);
        Vector3 randomDirection = Random.insideUnitSphere * randomDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomDistance, NavMesh.AllAreas))
        {
            //navMeshAgent.SetDestination(hit.position);
            destinationPosition = hit.position;
            navMeshAgent.SetDestination(destinationPosition);
        }
    }

    private bool CanAttack()
    {
        // ���� ������ Ȯ���Ͽ� ���� �������� ���θ� ��ȯ
        return !isAttacking && Time.time - lastAttackTime >= attackInterval;
    }

    private void Attack()
    {
        // ���� �ִϸ��̼� ��� �� ���� ����
        animator.SetTrigger("Attack");
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        // ���� ���� �� ���� ������ ��ٸ�
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); // �ִϸ��̼ǿ��� ������ ���۵Ǵ� �ð��� ���� ����

        // �÷��̾ ���� detectionRange �ȿ� �ִ��� Ȯ��
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // ���� ����� ���� detectionRange �ȿ� ������ ���ظ� ����
            Player playerHealth = player.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.Damaged(attackDamage);
            }
        }

        lastAttackTime = Time.time;
        isAttacking = false;
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
        Instantiate(meatPrefab, meatSpawnPosition, Quaternion.identity);
    }
}