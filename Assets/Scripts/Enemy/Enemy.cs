using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    EnemeyBaseState currentState;

    public Animator anim;
    public int animState;

    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;
    public Transform pointA, pointB;
    public Transform targetPoint;

    [Header("Attack Setting")]
    public float attackRate;
    public float attackRange, skillRange;

    private float nextAtack = 0;

    public List<Transform> attackList = new List<Transform>(); // 周围的可攻击对象

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        TransitionToState(patrolState);
        if (isBoss)
        {
            UIManager.instance.SetBossHealth(health);
        }
    }

    public virtual void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
            return;
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);

        if (isBoss)
        {
            UIManager.instance.UpdateBossHealth(health);
        }
    }

    public void TransitionToState(EnemeyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();
    }


    public void AttackAction() { // 攻击玩家
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAtack)
            {
                // 播放攻击动画
                anim.SetTrigger("attack");
                nextAtack = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction() // 对炸弹使用技能
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAtack)
            {
                // 播放技能动画
                anim.SetTrigger("skill");
                nextAtack = Time.time + attackRate;
            }
        }
    }

    public void FlipDirection()
    {
        if (targetPoint.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void SwitchPoint() {
        if (distanceX(pointA, transform) > distanceX(pointB, transform))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    public float distanceX(Transform a, Transform b)
    {
        return Mathf.Abs(a.position.x - b.position.x);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!attackList.Contains(collision.transform) && !hasBomb && !GameManager.instance.isGameOver)
        {
            attackList.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attackList.Contains(collision.transform))
        {
            attackList.Remove(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("is Dead: " + isDead + ", gameOver: " + GameManager.instance.isGameOver);
        if (isDead || GameManager.instance.isGameOver)
            return;
        StartCoroutine(OnAlarm());
    }

    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}
