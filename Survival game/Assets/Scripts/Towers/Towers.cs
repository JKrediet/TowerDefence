using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    public float rotationSpeed, attackSpeed, damage, bulletSpeed, towerRange, projectileCount, critChance, critDamage, chain, shootAngle;
    private float BaseRotationSpeed, BaseAttackSpeed, BaseDamage, BaseBulletSpeed, BaseTowerRange, baseProjectileCount, baseCritChance, baseCritDamage, baseChain,
        itemConbined_rotationSpeed, itemConbined_attackSpeed, itemConbined_damage, itemConbined_bulletSpeed, itemConbined_towerRange, itemCombined_baseProjectileCount, itemCombined_critChance, itemCombined_critDamage, itemCombined_chain;
    public Rigidbody bullet;
    public List<Transform> targets;
    public LayerMask mask;
    public List<GameObject> items;
    public bool item1, item2, item3;
    protected float total, min, max;
    public int element, elementSlot1, elementSlot2, elementSlot3;

    //privates
    protected float attackTime, nextAttack, distanceCheck, enemyDistance;
    protected Transform shootThis;

    private void Start()
    {
        //set base stats
        BaseRotationSpeed = rotationSpeed;
        BaseAttackSpeed = attackSpeed;
        BaseDamage = damage;
        BaseBulletSpeed = bulletSpeed;
        BaseTowerRange = towerRange;
        baseProjectileCount = projectileCount;
        baseCritChance = critChance;
        baseCritDamage = critDamage;
        baseChain = chain;
    }
    protected virtual void Update()
    {
        if (shootThis == null)
        {
            distanceCheck = towerRange;
            if (targets.Count > 0)
            {
                targets.RemoveAll(x => x == null);
                for (int i = 0; i < targets.Count; i++)
                {
                    enemyDistance = Vector3.Distance(transform.position, targets[i].transform.position);
                    if (enemyDistance <= distanceCheck)
                    {
                        distanceCheck = enemyDistance;
                        shootThis = targets[i];
                    }
                }
            }
        }
        else
        {
            AimAtTarget();
            Fire();
        }
    }
    protected virtual void AimAtTarget()
    {
        Vector3 pos = (shootThis.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
    protected void OnTriggerEnter(Collider _target)
    {
        if (_target.gameObject.CompareTag("Enemy"))
        {
            targets.Add(_target.transform);
        }
    }
    protected void OnTriggerExit(Collider _target)
    {
        if (_target.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(_target.transform);
        }
        if(_target.transform == shootThis)
        {
            shootThis = null;
        }
    }
    protected virtual void Fire()
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out _hit, towerRange, mask))
        {
            if (Time.time > nextAttack)
            {
                attackTime = attackSpeed / (attackSpeed * attackSpeed);
                nextAttack = attackTime + Time.time;

                for (int i = 0; i < projectileCount; i++)
                {
                    min = transform.rotation.y - shootAngle;
                    max = transform.rotation.y + shootAngle; //(max - min)
                    total = (max - min) / projectileCount;
                    float value = (float)(Mathf.Atan2(transform.rotation.y, transform.rotation.w) / Mathf.PI) * 180;
                    if (value > 180)
                    {
                        value -= 360;
                    }
                    Rigidbody clone;
                    clone = Instantiate(bullet, transform.position + transform.up, Quaternion.Euler(new Vector3(0, value - (total * (projectileCount / 2)) + (total * (i + 0.5f)), 0)));
                    //roll crit
                    float critValue = Random.Range(0, 101);
                    float critMulti = critDamage * 0.01f;
                    if (critChance > critValue)
                    {
                        clone.GetComponent<BulletDamage>().damage = damage * critMulti;
                        clone.GetComponent<BulletDamage>().critTrue = true;
                    }
                    else
                    {
                        clone.GetComponent<BulletDamage>().damage = damage;
                    }
                    clone.GetComponent<BulletDamage>().element = element;
                    clone.GetComponent<BulletDamage>().shooter = gameObject;
                    clone.GetComponent<BulletDamage>().chainCount = (int)chain;
                    clone.GetComponent<BulletDamage>().projectileSpeed = bulletSpeed;
                    clone.GetComponent<BulletDamage>().objectsHit = new List<Transform>(0);
                    clone.velocity = clone.transform.TransformDirection(clone.transform.forward) * bulletSpeed;
                }
            }
        }
    }
    public void UpdateItemStats()
    {
        foreach(GameObject item in items)
        {
            if (item != null)
            {
                ItemValue it = item.GetComponent<ItemValue>();
                itemConbined_damage += it.damage;
                itemConbined_rotationSpeed += it.rotationSpeed;
                itemConbined_attackSpeed += it.attackSpeed;
                itemConbined_bulletSpeed += it.bulletSpeed;
                itemConbined_towerRange += it.range;
                itemCombined_baseProjectileCount += it.projectileCount;
                itemCombined_critChance += it.critChance;
                itemCombined_critDamage += it.critDamage;
                itemCombined_chain += it.chain;
                //element
                if (it.numberInInventory == 0)
                {
                    elementSlot1 = it.element;
                    if (elementSlot1 == elementSlot2 || elementSlot1 == elementSlot3)
                    {
                        element = it.GetComponent<ItemValue>().element;
                    }
                }
                if (it.numberInInventory == 1)
                {
                    elementSlot2 = it.element;
                    if (elementSlot2 == elementSlot1 || elementSlot2 == elementSlot3)
                    {
                        element = it.GetComponent<ItemValue>().element;
                    }
                }
                if (it.numberInInventory == 2)
                {
                    elementSlot3 = it.element;
                    if (elementSlot3 == elementSlot1 || elementSlot3 == elementSlot2)
                    {
                        element = it.GetComponent<ItemValue>().element;
                    }
                }
            }
        }
        //damage
        damage = BaseDamage + itemConbined_damage;
        itemConbined_damage = 0;
        //rotationSpeed
        rotationSpeed = BaseRotationSpeed + itemConbined_rotationSpeed;
        itemConbined_rotationSpeed = 0;
        //attackSpeed
        attackSpeed = BaseAttackSpeed + itemConbined_attackSpeed;
        itemConbined_attackSpeed = 0;
        //bulletSpeed
        bulletSpeed = BaseBulletSpeed + itemConbined_bulletSpeed;
        itemConbined_bulletSpeed = 0;
        //towerRange
        towerRange = BaseTowerRange + itemConbined_towerRange;
        itemConbined_towerRange = 0;
        //projectileCount
        projectileCount = baseProjectileCount + itemCombined_baseProjectileCount;
        itemCombined_baseProjectileCount = 0;
        //critchance
        critChance = baseCritChance + itemCombined_critChance;
        itemCombined_critChance = 0;
        //critDamage
        critDamage = baseCritDamage + itemCombined_critDamage;
        itemCombined_critDamage = 0;
        //chain
        chain = baseChain + itemCombined_chain;
        itemCombined_chain = 0;

        GetComponent<SphereCollider>().radius = towerRange;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + transform.up, transform.forward * towerRange);
    }
}
