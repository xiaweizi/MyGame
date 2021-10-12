using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 巡逻状态
public class PatrolState : EnemeyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        bool animIdlePlaying = enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle");
        if (!animIdlePlaying)
        {
            // 如果正在播放
            enemy.animState = 1;
            enemy.MoveToTarget();
        }

        if (enemy.distanceX(enemy.transform, enemy.targetPoint) <= 0.01f)
        {
            enemy.TransitionToState(enemy.patrolState);
        }

        if (enemy.attackList.Count > 0)
        {
            enemy.TransitionToState(enemy.attackState);
        }

    }
}
