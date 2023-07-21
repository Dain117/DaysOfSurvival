using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public float normalSpeed = 3f; // 늑대의 기본 속도
    public float runSpeed = 5f; // 늑대의 도망 속도
    public float detectionRange = 8f; // 플레이어를 감지하는 거리
    public int maxHP = 30; // 늑대의 최대 체력
    public int attackDamage = 5; // 늑대의 공격력
    public float attackInterval = 3f; // 늑대의 공격 간격

    public GameObject meatPrefab;

    private Transform player; // 플레이어의 위치
    private int currentHP; // 늑대의 현재 체력
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 태그로 찾아서 할당
        currentHP = maxHP; // 현재 체력을 최대 체력으로 설정

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");

        // 늑대의 초기 목적지 설정
        SetRandomDestination();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 일정 거리 안에 들어왔을 때
        if (distanceToPlayer <= detectionRange)
        {
            // 플레이어를 따라가도록 설정
            ChangeSpeed(runSpeed);
            animator.SetTrigger("Run");
            //navMeshAgent.SetDestination(player.position);
            navMeshAgent.SetDestination(player.position);

            // 공격 가능한지 확인하고 공격
            if (CanAttack())
            {
                Attack();
            }
        }
        else
        {
            // 플레이어가 detectionRange를 벗어났을 때
            // 늑대는 기본 속도로 돌아가고 랜덤한 목적지로 이동
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
        // NavMeshAgent의 속도를 변경하는 메소드
        navMeshAgent.speed = speed;
    }

    private void SetRandomDestination()
    {
        // 랜덤한 위치로 이동할 목적지 설정
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);
    }

    private bool CanAttack()
    {
        // 공격 간격을 확인하여 공격 가능한지 여부를 반환
        return !isAttacking && Time.time - lastAttackTime >= attackInterval;
    }

    private void Attack()
    {
        // 공격 애니메이션 재생 및 공격 실행
        animator.SetTrigger("Attack");
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        // 공격 실행 후 공격 간격을 기다림
        isAttacking = true;
        yield return new WaitForSeconds(0.5f); // 애니메이션에서 공격이 시작되는 시간에 맞춰 조정

        // 플레이어가 아직 detectionRange 안에 있는지 확인
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // 공격 대상이 아직 detectionRange 안에 있으면 피해를 입힘
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
