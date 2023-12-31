using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Rabbit : MonoBehaviour
{
    public float normalSpeed = 1f; // 토끼의 기본 속도
    public float runSpeed = 3f; // 토끼의 도망 속도
    public float detectionRange = 5f; // 플레이어를 감지하는 거리
    public int maxHP = 20; // 토끼의 최대 체력

    public GameObject meatPrefab; //고기 오브젝트의 프리팹

    private Transform player; // 플레이어의 위치
    private int currentHP; // 토끼의 현재 체력
    private NavMeshAgent navMeshAgent;
    private Animator animator;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 태그로 찾아서 할당
        currentHP = maxHP; // 현재 체력을 최대 체력으로 설정

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = normalSpeed;

        animator = GetComponent<Animator>();
        animator.SetTrigger("Idle");

        // 토끼의 초기 목적지 설정
        SetRandomDestination();
    }

   private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 일정 거리 안에 들어왔을 때
        if (distanceToPlayer <= detectionRange)
        {
            // 도망 상태로 변경하고 도망 속도로 설정
            ChangeSpeed(runSpeed);
            animator.SetTrigger("Run");

            // 플레이어의 반대 방향으로 도망
            Vector3 direction = transform.position - player.position;
            direction.y = 0f;
            navMeshAgent.SetDestination(transform.position + direction.normalized * 10f);

        }
        else
        {
            // 도망 상태 해제하고 기본 속도로 설정
            ChangeSpeed(normalSpeed);
            animator.SetTrigger("Walk");
            //navMeshAgent.destination = player.position;
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                SetRandomDestination();
            }
        }

        // 도착 지점에 도달하면 새로운 목적지 설정
       
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
