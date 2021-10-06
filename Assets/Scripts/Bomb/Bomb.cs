using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private Animator animator;
    private Collider2D coll;

    public float startTime;
    public float waitTime;
    public float bombForce;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;


    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Time.time > startTime + waitTime)
        {
            // 当满足条件是，则触发爆炸动画
            animator.Play("bomb_explotion");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Explotion() // animation event
    {
        // 爆炸动画第一帧，开始给周边的施加力
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        foreach (var item in aroundObjects)
        {
            if (item != coll)
            {
                // 炸弹本身自己，不需要施加作用力
                Vector3 pos = transform.position - item.transform.position;
                item.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * bombForce, ForceMode2D.Impulse);// 冲击力
            }

        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
