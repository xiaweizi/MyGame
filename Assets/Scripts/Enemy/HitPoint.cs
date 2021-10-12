using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private int dir;
    public bool bombAvilabe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("eneter!!");
        if (transform.position.x > other.transform.position.x)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHit(1);
        }

        if (other.CompareTag("Bomb") && bombAvilabe)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * 10, ForceMode2D.Impulse);
        }
    }

    
}
