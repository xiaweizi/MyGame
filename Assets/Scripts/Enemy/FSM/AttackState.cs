using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemeyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.hasBomb)
            return;
        if (enemy.attackList.Count == 0)
        {
            enemy.TransitionToState(enemy.patrolState);
        }
        if (enemy.attackList.Count > 1) {
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                if (enemy.distanceX(enemy.transform, enemy.attackList[i]) < enemy.distanceX(enemy.transform, enemy.targetPoint))
                {
                    enemy.targetPoint = enemy.attackList[i];
                }
            }
        }
        if (enemy.attackList.Count == 1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            enemy.SkillAction();
        }

        enemy.MoveToTarget();
    }
}
