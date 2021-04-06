using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BulletDamage : MonoBehaviour
{
    public float damage, projectileSpeed;
    public Rigidbody newProjectile, rb;
    public GameObject shooter;
    public int chainCount, element;
    public List<Transform> objectsHit;
    public LayerMask mask;
    public bool critTrue;

    //element
    public float burnDamage, burnDuration, lightningChainDamage, lightningChainAmount, lightningRange, freezeDuration, freezeChance, freezeSlow;

    private void Start()
    {
        Destroy(gameObject, 10);
        ElementCheck();
        rb = GetComponent<Rigidbody>();
    }
    //0 = nothing, 1 = lightning, 2 = water, 3 = earth, 4 = ice, 5 = nature, 6 = fire
    public void ElementCheck()
    {
        if (element == 0)
        {
            GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
        }
        else if (element == 1)
        {
            //lightning
            GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 0, 255);
            lightningChainDamage = damage * 0.75f;
            lightningChainAmount = 5;
            lightningRange = 5;
        }
        else if (element == 2)
        {
            //water
            GetComponent<MeshRenderer>().material.color = new Color32(0, 0, 255, 255);
        }
        else if (element == 3)
        {
            //earth
            GetComponent<MeshRenderer>().material.color = new Color32(139, 69, 19, 255);
        }
        else if (element == 4)
        {
            //ice
            GetComponent<MeshRenderer>().material.color = new Color32(173, 216, 230, 255);
            freezeDuration = 2;
            freezeChance = 10;
            freezeSlow = 30;
        }
        else if (element == 5)
        {
            //nature
            GetComponent<MeshRenderer>().material.color = new Color32(0, 255, 0, 255);
        }
        else if (element == 6)
        {
            //fire
            GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 255);
            burnDamage = damage * 0.25f;
            burnDuration = 2;
        }
    }

    private void OnTriggerEnter(Collider _hit)
    {
        if (shooter)
        {
            if (shooter.GetComponent<Towers>() || shooter.GetComponent<PlayerHealth>())
            {
                if (_hit.GetComponent<EnemyBehavior>())
                {
                    if (objectsHit.Contains(_hit.transform))
                    {
                        GiveDamage(_hit.GetComponent<BaseHealth>());
                    }
                    else
                    {
                        if (chainCount > 0)
                        {
                            rb.velocity = Vector3.zero;
                            transform.position = _hit.transform.position;
                            objectsHit.Add(_hit.transform);
                            chainCount--;
                            damage *= 0.5f;
                            _hit.GetComponent<BaseHealth>().DoDamage(damage, critTrue, burnDamage, burnDuration, freezeDuration, freezeSlow, freezeChance, lightningChainDamage, lightningChainAmount, lightningRange);
                            Collider[] enemies = Physics.OverlapSphere(transform.position, Mathf.Infinity, mask);
                            if (enemies.Length == 0)
                            {
                                Destroy(gameObject);
                            }
                            Vector3 pos = new Vector3(0, 0, 0);
                            float distanceCheck = 1000;
                            for (int i = 0; i < enemies.Length; i++)
                            {
                                if (!objectsHit.Contains(enemies[i].transform))
                                {
                                    float enemyDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
                                    if (enemyDistance <= distanceCheck)
                                    {
                                        distanceCheck = enemyDistance;
                                        pos = (enemies[i].transform.position - transform.position).normalized; ;
                                    }
                                }
                            }
                            if (distanceCheck == 1000)
                            {
                                Destroy(gameObject);
                            }
                            transform.rotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
                            rb.velocity = transform.forward * projectileSpeed;
                        }
                        else
                        {
                            GiveDamage(_hit.GetComponent<BaseHealth>());
                        }
                    }
                }
            }
            if (shooter.GetComponent<RangedEnemy>())
            {
                if (_hit.GetComponent<COREHealth>())
                {
                    GiveDamage(_hit.GetComponent<BaseHealth>());
                }
            }
        }
    }
    private void GiveDamage(BaseHealth health)
    {
        health.DoDamage(damage, critTrue, burnDamage, burnDuration, freezeDuration, freezeSlow, freezeChance, lightningChainDamage, lightningChainAmount, lightningRange);
        Destroy(gameObject);
    }
}
