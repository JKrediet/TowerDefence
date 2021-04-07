using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseHealth : MonoBehaviour
{
    public float Health;
    protected float Maxhealth;

    //enemy
    EnemyBehavior enemyController;

    //damage pop up
    public GameObject damagePopUp, critPopUp;
    public Vector3 height;

    //burn damage
    public float totalBurnDuration;
    protected float nextBurnCall, burnCooldown = 0.24f;
    public GameObject FireBurn;

    //freeze
    public float totalFreezeDuration;
    public bool isFrozen;
    public GameObject freezeSlowObject, freezeIce;

    //lightning
    public LayerMask mask;
    public List<Transform> objectsHit;
    public LineRenderer line;

    protected virtual void Start()
    {
        Maxhealth = Health;
        objectsHit = new List<Transform>();
        if (GetComponent<EnemyBehavior>())
        {
            enemyController = GetComponent<EnemyBehavior>();
        }
    }                           //damage        crit        burn Damage     burn duration       freeze duration         slow                freeze chance    lightning dmaage           //lghnintg chain amount   lightning range
    public virtual void DoDamage(float amount, bool crit, float burnDamage, float burnDuration, float freezeDuration, float freezeSlow, float freezeChance, float lightningDamage, float lightningChainAmount, float lightningRange)
    {
        Health -= amount;

        //elemental effects
        BurnStuff(burnDuration, burnDamage, crit);
        FreezeStuff(freezeDuration, freezeSlow, freezeChance);
        StartCoroutine(LightningStuff(lightningDamage, lightningChainAmount, lightningRange, crit, freezeSlow, freezeChance));

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    //burn
    public IEnumerator TakeBurnDamage(float damage, bool crit)
    {
        totalBurnDuration -= 0.25f;
        yield return new WaitForSeconds(0.25f);
        DoDamage(damage, crit, damage, 0, 0, 0, 0,0,0,0);
    }
    //fire
    public void BurnStuff(float burnDuration, float damage, bool crit)
    {
        totalBurnDuration += burnDuration;
        if (totalBurnDuration > 0)
        {
            if (damage > 1)
            {
                if (Time.time > nextBurnCall)
                {
                    nextBurnCall = Time.time + burnCooldown;
                    StartCoroutine(TakeBurnDamage(damage, crit));
                }
            }
        }
    }
    //ice
    public void FreezeStuff(float freezeDuration, float freezeSlow, float freezeChance)
    {
        //slow
        if (enemyController)
        {
            totalFreezeDuration += freezeDuration;
            if (totalFreezeDuration > 0)
            {
                if (!isFrozen)
                {
                    enemyController.agent.speed = enemyController.movementSpeed * ((100 - freezeSlow) / 100);
                }
            }

            if (!isFrozen)
            {
                //freeze
                int roll = Random.Range(0, 99);
                if (roll < freezeChance)
                {
                    isFrozen = true;
                    freezeIce.SetActive(isFrozen);
                    enemyController.agent.speed = 0;
                    Invoke("UnFreeze", 2);
                }
            }
        }
    }
    public void UnFreeze()
    {
        isFrozen = false;
        freezeIce.SetActive(isFrozen);
    }
    //lightning
    public IEnumerator LightningStuff(float lightningDamage, float lightningChainAmount, float lightningRange, bool crit, float freezeSlow, float freezeChance)
    {
        if(lightningChainAmount > 0)
        {
            objectsHit.Add(gameObject.transform);
            Collider[] enemies = Physics.OverlapSphere(transform.position, lightningRange, mask);
            float distanceCheck = 1000;
            Transform NextChainTarget = null;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    if (!objectsHit.Contains(enemies[i].transform))
                    {
                        float enemyDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
                        if (enemyDistance <= distanceCheck)
                        {
                            distanceCheck = enemyDistance;
                            NextChainTarget = enemies[i].transform;
                        }
                    }
                }
            }
            if (lightningDamage > 1)
            {
                if (NextChainTarget != null)
                {
                    if (transform != null)
                    {
                        //line inspawnen 
                        LineRenderer lijntje = Instantiate(line, transform);
                        lijntje.SetPosition(0, transform.position);
                        lijntje.SetPosition(1, NextChainTarget.position);
                        Destroy(lijntje, 0.1f);
                        yield return new WaitForSeconds(0.1f);
                        //damage
                        if (NextChainTarget != null)
                        {
                            NextChainTarget.GetComponent<BaseHealth>().objectsHit = new List<Transform>(objectsHit);
                            float freeze = 0;
                            float burn = 0;
                            if (totalFreezeDuration > 0)
                            {
                                freeze = 2;
                            }
                            if (totalBurnDuration > 0)
                            {
                                burn = 2;
                            }
                            NextChainTarget.GetComponent<BaseHealth>().DoDamage(lightningDamage, crit, lightningDamage * 0.1f, burn, freeze, freezeSlow, freezeChance, lightningDamage * 0.5f, lightningChainAmount--, lightningRange);
                        }
                    }
                }
            }
            objectsHit.Clear();
        }
    }
    private void Update()
    {
        if (FireBurn)
        {
            if (totalBurnDuration > 0)
            {
                FireBurn.SetActive(true);
            }
            else
            {
                FireBurn.SetActive(false);
            }
        }
        if (enemyController)
        {
            if (totalFreezeDuration > 0)
            {
                freezeSlowObject.SetActive(true);
                totalFreezeDuration -= Time.deltaTime;
            }
            else
            {
                enemyController.agent.speed = enemyController.movementSpeed;
                freezeSlowObject.SetActive(false);
            }
        }
    }
}
