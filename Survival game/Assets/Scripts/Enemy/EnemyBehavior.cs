using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public float attackDistance, attackSpeed, damage, bulletSpeed, movementSpeed;
    protected GameObject destination;
    protected float attackTime, nextAttack;
    protected bool mayMove = true;
    protected Animator anim;
    public NavMeshAgent agent;
    public Rigidbody bullet;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        destination = GameObject.FindGameObjectWithTag("CORE");
        anim = GetComponent<Animator>();
    }
    protected virtual void FixedUpdate()
    {
        Movement();
    }
    protected virtual void Movement()
    {
        if (mayMove)
        {
            agent.SetDestination(destination.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
    protected virtual void Attack()
    {
    }
    protected virtual void DoneAttacking()
    {
    }
}
