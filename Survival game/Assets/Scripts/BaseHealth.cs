using UnityEngine;
using System.Collections;

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
    public GameObject freezeSlowObject;

    protected virtual void Start()
    {
        Maxhealth = Health;
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
        LightningStuff(lightningDamage, lightningChainAmount, lightningRange);

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
                enemyController.agent.speed = 0;
            }
        }
    }
    //lightning
    public void LightningStuff(float lightningDamage, float lightningChainAmount, float lightningRange)
    {
        if(lightningChainAmount > 0)
        {

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
