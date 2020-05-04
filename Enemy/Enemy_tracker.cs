using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy_tracker : LivingEntity
{
    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }

    private State state;

    private NavMeshAgent agent;
    private Animator animator; 

    public Transform attackRoot; //공격한 피봇포인트, 해당 반경내 물체공격받음
    public Transform eyeTransform; // 시야의 기준점, 영역지정해서 다른거 감지

    //private AudioSource audioPlayer; // 소리
    //public AudioClip hitClip; //타격당했을때
    //public AudioClip deathClip; //죽을때
    public ParticleSystem explosion;
    //private Renderer skinRenderer; //피부색을 공격에 다르게

    public float runSpeed = 10f; //이동속도
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f; //방향회전시 드는 지연시간
    private float turnSmoothVelocity; // 실시간 회전의 변화량

    public float damage = 1.0f;
    public float attackRadius = 2f;
    private float attackDistance;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f; //평소 속도

    public LivingEntity targetEntity; //추적대상
    public LayerMask whatIsTarget; //적인지 감별
    
    private RaycastHit[] hits = new RaycastHit[10]; //범위기반의 공격은 여러 충돌포인트 생김
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>(); // 공격이 2번이상 적용되지 않도록

    private bool hasTarget => targetEntity != null && !targetEntity.dead;

    public Slider healthSlider;

    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
            //enemy의 공격이 들어가는 범위
        }
        
        //적의 시야 범위는 arc
        if (eyeTransform != null)
        {
            var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            var leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }
    }
#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //audioPlayer = GetComponent<AudioSource>();
        //skinRenderer = GetComponentInChildren<Renderer>();
        
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;
        //Debug.Log("attackDistance: " + attackDistance);
        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
        
    }

    //적의 스펙
    public void Setup(float health, float damage, 
        float runSpeed, float patrolSpeed, Color skinColor)
    {
        startHealth = health; //living entity <- input
        this.health = health; //

        this.damage = damage;
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;

        //skinRenderer.material.color = skinColor;
        agent.speed = patrolSpeed;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdatePath());
    }


    // 애니메이션, 거리 계산해서 공격
    void Update()
    {
        
        if (dead)
        {
            return;
        }

        if (state == State.Tracking)
        {
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            //Debug.Log("distance btw base and rabbit: " + distance);
            if (distance <= attackDistance)
            {
                //Debug.Log("BeginAttack when distance: "+distance+", attackDistance: "+attackDistance);
                BeginAttack();
            }
            else
            {
                // 플레이어가 도망가면 쫓아오지 못하게.
                targetEntity = null;
            }
        }
        //animator.SetTrigger("Forward");
        animator.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }

        private void FixedUpdate()
    {
        if (dead) return;
        
        //대상을 바라보게 하는 코드
        if (state == State.AttackBegin || state == State.Attacking)
        {
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            //y축회전만 고려
            //스르륵 바라보기
            var targetAngleY = lookRotation.eulerAngles.y;
            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, 
                targetAngleY, ref turnSmoothVelocity,turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }

        //공격궤적. 타임안에 들어오면 감지
        if (state == State.Attacking)
        {
            var direction = transform.forward;
            var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

            //감지 정확도 높이기
            var size = Physics.SphereCastNonAlloc(attackRoot.position, 
                attackRadius, direction, hits, deltaDistance, whatIsTarget);

            //Debug.Log("Stone size: "+size);
            //실제 damage주는 코드
            for (var i = 0; i < size; i++)
            {
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();
                //Debug.Log("Stone's attackTargetEntity: " + attackTargetEntity);
                if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
                {
                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damager = gameObject;
                    
                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }

                    message.hitNormal = hits[i].normal;
                    
                    attackTargetEntity.ApplyDamage(message);
                    lastAttackedTargets.Add(attackTargetEntity);
                    break;
                }
            }
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead) //살아있는동안
        {
            if (hasTarget) //추적대상을 찾아서
            {
                //Debug.Log("Stone has Target!");
                if (state == State.Patrol)
                {
                    // 정찰상태를 추적상태로 바꾼다
                    state = State.Tracking;
                    agent.speed = runSpeed;
                }
                //몬스터의 이동 목표위치를 추적상태로 정한다.
                agent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                //추적대상이 없을때
                //targetEntity가 null이거나 사망했을때
                //추적대상을 바꿀 수 있게 하는 코드
                if (targetEntity != null) targetEntity = null;
                
                //일단 추적상태가 아니므로 정찰상태로 바꿔주기
                if (state != State.Patrol)
                {
                    //Debug.Log("change to Patrol");
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }
                
                //랜덤생성되었던 정찰목표지점 까지의 거리와 가까워지면  
                //Debug.Log("remainDistance: " + agent.remainingDistance);
                if (agent.remainingDistance <= 1.8f) //1.0f이면 stopping distance 가 remainingDistance보다 커서 정지
                {
                    //Debug.Log("Looking for another spot");
                    //새로운 정찰지점 찾기
                    var patrolTargetPosition = Utility.GetRandomPointOnNavMesh(
                        transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolTargetPosition);
                    //Debug.Log("patrolTargetPosition: "+patrolTargetPosition);
                }
                
                //적이 추적대상을 시야로 감지하는 코드
                //whatIsTarget: player layer
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                    
                foreach (var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform))
                    {
                        // 구체 내에는 있지만 적의 시야(부채꼴)에 없으면 무시
                        continue;
                    }
                    
                    //Debug.Log("target is on sight!");
                    
                    //살아있는 상대방인지 확인, LivingEntity 얻어냄
                    var livingEntity = collider.GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity; // stone이니까 player가 됨
                        //Debug.Log("Stone mon's target is set");
                        break;
                    }
                }

            }
            yield return new WaitForSeconds(0.05f); 
        }
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        //공격 받기 불가능 상태
        if (!base.ApplyDamage(damageMessage)) return false;
        
        //아직 추적대상 못찾았는데 공격받으면 추적대상 바로 변경
        //if (targetEntity == null)
        //{
        //    targetEntity = damageMessage.damager.GetComponent<LivingEntity>();
        //}

        //EffectManager.Instance.PlayHitEffect(damageMessage.hitPoint, damageMessage.hitNormal, transform,
        //    EffectManager.EffectType.Flesh);
        //audioPlayer.PlayOneShot(hitClip);
        animator.SetTrigger("Damage");
        healthSlider.value = health;
        return true;
    }

    public void BeginAttack()
    {
        //대미지가 아직 들어가지는 않음
        state = State.AttackBegin;
        agent.isStopped = true;
        //Debug.Log("Stopped, BeginAttack");
        animator.SetTrigger("Attack");
    }
    public void EnableAttack()
    {
        //대미지 들어감
        //Debug.Log("EnableAttack");
        state = State.Attacking;
        lastAttackedTargets.Clear();
    }

    public void DisableAttack()
    {
        if (hasTarget)
        {
            //Debug.Log("Disable but has Target, Track!");
            state = State.Tracking;
        }
        else
        {
            //Debug.Log("Disable and no target, patrol");
            state = State.Patrol;
        }
        agent.isStopped = false;
    }
    private bool IsTargetOnSight(Transform target)
    {
        var direction = target.position - eyeTransform.position;
        direction.y = eyeTransform.forward.y; // 몬스터 눈높이에서 앞으로 일직선, 높이차이 고려x

        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            //추적 대상이 시야에서 벗어나서 false
            return false;
        }
        //추적대상이 시야에 있으면
        //방향 원래값으로 되돌려준다. 높이 차이 고려
        direction = target.position - eyeTransform.position;
        
        //추적대상이 시야에 있을 때
        RaycastHit hit;
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target) // 처음 검사한 상대방
            {
                return true;
            }
        }
        
        // 시야 안이지만 장애물 있을때
        return false;
    }

    public override void Die()
    {
        base.Die();
        GetComponent<Collider>().enabled = false;
        
        //agent.isStopped = true;
        agent.enabled = false;

        //죽을때 root 맡김 애니메이션에
        animator.applyRootMotion = true;
        animator.SetTrigger("Die");

        //Instantiate(explosion, this.transform.position, Quaternion.identity);

        Destroy(gameObject, 2.0f);

        //audioPlayer.PlayOneShot(deathClip);
    }
}
