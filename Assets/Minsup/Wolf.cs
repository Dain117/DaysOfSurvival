using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public float normalSpeed = 3f; // ������ �⺻ �ӵ�
    public float runSpeed = 5f; // ������ ���� �ӵ�
    public float detectionRange = 8f; // �÷��̾ �����ϴ� �Ÿ�
    public int maxHP = 30; // ������ �ִ� ü��
    public int attackDamage = 5; // ������ ���ݷ�
    public float attackInterval = 3f; // ������ ���� ����

    public GameObject meatPrefab;

    private Transform player; // �÷��̾��� ��ġ
    private int currentHP; // ������ ���� ü��
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾ �±׷� ã�Ƽ� �Ҵ�
        currentHP = maxHP; // ���� ü���� �ִ� ü������ ����

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");

        // ������ �ʱ� ������ ����
        SetRandomDestination();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ���� �Ÿ� �ȿ� ������ ��
        if (distanceToPlayer <= detectionRange)
        {
            // �÷��̾ ���󰡵��� ����
            ChangeSpeed(runSpeed);
            animator.SetTrigger("Run");
            //navMeshAgent.SetDestination(player.position);
            navMeshAgent.SetDestination(player.position);

            // ���� �������� Ȯ���ϰ� ����
            if (CanAttack())
            {
                Attack();
            }
        }
        else
        {
            // �÷��̾ detectionRange�� ����� ��
            // ����� �⺻ �ӵ��� ���ư��� ������ �������� �̵�
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
        // NavMeshAgent�� �ӵ��� �����ϴ� �޼ҵ�
        navMeshAgent.speed = speed;
    }

    private void SetRandomDestination()
    {
        // ������ ��ġ�� �̵��� ������ ����
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);
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
            PlayerDain playerHealth = player.GetComponent<PlayerDain>();
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
