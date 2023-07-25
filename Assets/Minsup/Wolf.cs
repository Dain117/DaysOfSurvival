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

    public float normalSpeed = 2f; // 늑대의 기본 속도
    public float runSpeed = 4f; // 늑대의 도망 속도
    public float detectionRange = 8f; // 플레이어를 감지하는 거리
    public int maxHP = 30; // 늑대의 최대 체력
    public int attackDamage = 5; // 늑대의 공격력
    public float attackInterval = 3f; // 늑대의 공격 간격
    public float attackRange = 2.7f;

    public GameObject meatPrefab;

    private Transform player; // 플레이어의 위치
    private int currentHP; // 늑대의 현재 체력
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
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 태그로 찾아서 할당
        currentHP = maxHP; // 현재 체력을 최대 체력으로 설정

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshPath = new NavMeshPath();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Walk");

        // 늑대의 초기 목적지 설정
        InvokeRepeating("SetRandomDestination", changeDestinationInterval, changeDestinationInterval);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (CanAttack())
            {
                // 플레이어가 일정 거리 안에 있고, 공격 가능한 상태라면 공격 상태로 변경
                currentState = WolfState.Attacking;
            }
            else
            {
                // 그 외에는 뛰어가는 상태로 변경
                currentState = WolfState.Running;
            }
        }
        else
        {
            // 플레이어가 일정 거리 밖에 있으면 Idle 상태로 변경
            currentState = WolfState.Walking;
        }

        // 상태에 따라 동작을 수행합니다.
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
            // 장애물 회피 로직 추가
            navMeshAgent.CalculatePath(player.position, navMeshPath);
            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                // 장애물이 없는 경로가 있을 경우, 플레이어 위치로 이동
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                // 장애물이 있어서 경로가 막혀있는 경우, 랜덤한 위치로 이동
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
        // NavMeshAgent의 속도를 변경하는 메소드
        navMeshAgent.speed = speed;
    }

    private void SetRandomDestination()
    {
        // 랜덤한 위치로 이동할 목적지 설정
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