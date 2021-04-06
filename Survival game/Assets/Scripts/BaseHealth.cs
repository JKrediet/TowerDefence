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
    protected bool isFrozen;
    public GameObject freezeSlowObject, freezeIce;

    //lightning
    public LayerMask mask;
    public List<Transform> objectsHit;

    protected virtual void Start()
    {
        Maxhealth = Health;
        objectsHit = new List<Transform>();
        if (GetComponent<EnemyBehavior>())
        {
            enemyController = GetComponent<EnemyBehavior>();
        }
    }                           //damage        crit        burn Damage     burn duration       freeze duration         slow                freeze chance    lightning dmaage
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
            if (Time.time > nextBurnCall)
            {
                nextBurnCall = Time.time + burnCooldown;
                StartCoroutine(TakeBurnDamage(damage, crit));
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
                if (roll < freezeSlow)
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
            if (NextChainTarget != null)
            {
                yield return new WaitForSeconds(0.1f);
                NextChainTarget.GetComponent<BaseHealth>().objectsHit = new List<Transform>(objectsHit);
                NextChainTarget.GetComponent<BaseHealth>().DoDamage(lightningDamage, crit, lightningDamage * 0.1f, totalBurnDuration, totalFreezeDuration, freezeSlow, freezeChance, lightningDamage * 0.5f, lightningChainAmount--, lightningRange);
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
