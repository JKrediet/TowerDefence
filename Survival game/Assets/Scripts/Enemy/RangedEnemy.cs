using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyBehavior
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
        if (distance < attackDistance)
        {
            if (Time.time > nextAttack)
            {
                mayMove = false;
                attackTime = attackSpeed / (attackSpeed * attackSpeed);
                nextAttack = attackTime + Time.time;
                Attack();
            }
        }
    }
    protected override void Attack()
    {
        Rigidbody clone = Instantiate(bullet, transform.position, transform.rotation);
        clone.velocity = clone.transform.forward * bulletSpeed;
        clone.GetComponent<BulletDamage>().damage = damage;
        clone.GetComponent<BulletDamage>().shooter = gameObject;
    }
    protected override void DoneAttacking()
    {
        anim.SetBool("Attack", false);
    }
}
