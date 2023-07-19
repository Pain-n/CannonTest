using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
    public Rigidbody rb;
    private GameContext context;

    public void Init(GameContext _context)
    {
        context = _context;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ground")
        {
            gameObject.SetActive(false);
        }

        if (collision.transform.tag == "Enemy")
        {
            for(int i = 0; i < context.EnemiesOnField.Count; i++)
            {
                if(collision.gameObject == context.EnemiesOnField[i].gameObject)
                {
                    context.EnemiesOnField[i].TakeDamage(context.Cannon.Damage);
                    break;
                }
            }
            gameObject.SetActive(false);
        }
    }
}
