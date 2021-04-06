using UnityEngine;
using System.Collections;

public class MeleeEnemy : EnemyBehavior
{
    protected override void FixedUpdate()
    {
        Movement();
        if (Time.time > nextAttack)
        {
            mayMove = true;
        }
    }
    protected override void Movement()
    {
        base.Movement();
        float distance = Vector3.Distance(transform.position, destination.transform.position);
        if(distance < attackDistance)
        {
            if(Time.time > nextAttack)
            {
                mayMove = false;
                attackTime = attackSpeed / (attackSpeed * attackSpeed);
                nextAttack = attackTime + Time.time;
                anim.SetBool("Attack", true);
            }
        }
    }
    protected override void Attack()
    {
        anim.SetBool("Attack", false);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1))
        {
            if(hit.transform.GetComponent<PlayerHealth>())
            {
                hit.transform.GetComponent<PlayerHealth>().DoDamage(damage, false, 0, 0,0,0,0,0,0,0);
            }
            if (hit.transform.GetComponent<COREHealth>())
            {
                hit.transform.GetComponent<COREHealth>().DoDamage(damage, false, 0, 0,0,0,0,0,0,0);
            }
        }
    }
    protected override void DoneAttacking()
    {
        anim.SetBool("Attack", false);
    }
}
