using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamageable
{

    public float scale;
    public List<GameObject> swalowBomb = new List<GameObject>();

    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public void Swalow() // Animation Event
    {
        if (swalowBomb.Count >= 3)
        {
            health = 0;
            isDead = true;
            anim.SetTrigger("hit");
            int x = 1;
            if (transform.localScale.x > 0)
            {
                x = 1;
            } else
            {
                x = -1;
            }
            transform.localScale = new Vector3(x, 1, 1);

            // 所有炸弹都放出
            foreach (var item in swalowBomb)
            {
                item.SetActive(true);
                item.GetComponent<Bomb>().TurnOn();
            }
            return;
        }

        targetPoint.GetComponent<Bomb>().TurnOff();
        targetPoint.gameObject.SetActive(false);
        if (!swalowBomb.Contains(targetPoint.gameObject))
        {
            swalowBomb.Add(targetPoint.gameObject);
        }

        transform.localScale *= scale;
    }
}
