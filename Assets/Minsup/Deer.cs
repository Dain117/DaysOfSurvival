using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Deer : MonoBehaviour
{
    public float normalSpeed = 4f; // �罿�� �⺻ �ӵ�
    public float runSpeed = 6f; // �罿�� ���� �ӵ�
    public float detectionRange = 8f; // �÷��̾ �����ϴ� �Ÿ�
    public int maxHP = 40; // �罿�� �ִ� ü��

    public GameObject meatPrefab; //��� ������Ʈ�� ������

    private Transform player; // �÷��̾��� ��ġ
    private int currentHP; // �罿�� ���� ü��
    private NavMeshAgent navMeshAgent;
    private Animator animator;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾ �±׷� ã�Ƽ� �Ҵ�
        currentHP = maxHP; // ���� ü���� �ִ� ü������ ����

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");

        // �罿�� �ʱ� ������ ����
        SetRandomDestination();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ���� �Ÿ� �ȿ� ������ ��
        if (distanceToPlayer <= detectionRange)
        {
            // ���� ���·� �����ϰ� ���� �ӵ��� ����
            ChangeSpeed(runSpeed);
            animator.SetTrigger("Run");

            // �÷��̾��� �ݴ� �������� ����
            Vector3 direction = transform.position - player.position;
            direction.y = 0f;
            navMeshAgent.SetDestination(transform.position + direction.normalized * 10f);

        }
        else
        {
            // ���� ���� �����ϰ� �⺻ �ӵ��� ����
            ChangeSpeed(normalSpeed);
            animator.SetTrigger("Walk");
            //navMeshAgent.destination = player.position;
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
        for(int i = 0; i<3; i++)
        {
            Vector3 meatSpawnPosition = transform.position + Vector3.up + Random.insideUnitSphere * 0.5f;
            Quaternion meatSpawnRotation = Quaternion.identity;

            GameObject meatObject = Instantiate(meatPrefab, meatSpawnPosition, Quaternion.identity);
        }
        
    }
}
