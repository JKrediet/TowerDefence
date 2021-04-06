using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject newItem;
    public List<Transform> itemSlots;
    private float damageRoll = 0;
    private float attackSpeedRoll = 0;
    private float rotationSpeedRoll = 0;
    private float rangeRoll = 0;
    private float projectileSpeedRoll = 0;
    private float projectileCountRoll = 0;
    private float critChanceRoll = 0;
    private float critDamageRoll = 0;
    private float chainRoll;
    private float elementRoll;
    public Button buy;
    public TextMeshProUGUI cost;

    private List<GameObject> createdItems = new List<GameObject>();

    private void Start()
    {
        GiveStatsToItem();
    }
    public void GiveStatsToItem()
    {
        for(int i = 0; i < itemSlots.Count; i++)
        {
            RollStats();

            GameObject createdItem = Instantiate(newItem, itemSlots[i].position, itemSlots[i].rotation, itemSlots[i]);

            ItemValue itemStats = createdItem.GetComponent<ItemValue>();
            itemStats.damage = damageRoll;
            itemStats.attackSpeed = attackSpeedRoll;
            itemStats.rotationSpeed = rotationSpeedRoll;
            itemStats.range = rangeRoll;
            itemStats.bulletSpeed = projectileSpeedRoll;
            itemStats.projectileCount = projectileCountRoll;
            itemStats.critChance = critChanceRoll;
            itemStats.critDamage = critDamageRoll;
            itemStats.chain = chainRoll;
            itemStats.element = (int)elementRoll;

            itemStats.isHoldBy = 2;
            itemStats.numberInInventory = i;
            itemStats.goldCost = damageRoll + attackSpeedRoll * 2 + rotationSpeedRoll * 0.1f + rangeRoll + projectileSpeedRoll * 0.3f + projectileCountRoll * 2 + critChanceRoll * 0.5f + critDamageRoll * 0.2f;
            createdItems.Add(createdItem);
        }
    }
    public void RollStats()
    {
        damageRoll = 0;
        attackSpeedRoll = 0;
        rotationSpeedRoll = 0;
        rangeRoll = 0;
        projectileSpeedRoll = 0;
        projectileCountRoll = 0;
        critChanceRoll = 0;
        critDamageRoll = 0;
        chainRoll = 0;
        elementRoll = 0;

        float exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            damageRoll = Random.Range(5, 11);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            attackSpeedRoll = Random.Range(1, 4);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            rotationSpeedRoll = Random.Range(30, 61);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            rangeRoll = Random.Range(2, 6);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            projectileSpeedRoll = Random.Range(10, 21);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            projectileCountRoll = Random.Range(0, 3);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            critChanceRoll = Random.Range(10, 26);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            critDamageRoll = Random.Range(0, 51);
        }
        exaltedCurtain = Random.Range(0, 11);
        if (exaltedCurtain > 7)
        {
            chainRoll = Random.Range(0, 3);
        }

        //elements
        exaltedCurtain = Random.Range(0, 15);
        if(exaltedCurtain > 7)
        {
            exaltedCurtain = Random.Range(0, 7);
            elementRoll = exaltedCurtain;
        }

        //check if atleast all are zero
        if (damageRoll == 0)
        {
            if (attackSpeedRoll == 0)
            {
                if (rotationSpeedRoll == 0)
                {
                    if (rangeRoll == 0)
                    {
                        if (projectileSpeedRoll == 0)
                        {
                            if(projectileCountRoll == 0)
                            {
                                if(critChanceRoll == 0)
                                {
                                    if(critDamageRoll == 0)
                                    {
                                        if(chainRoll == 0)
                                        {
                                            //if all stats 0, reroll
                                            RollStats();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void DeleteItems()
    {
        if(createdItems.Count > 0)
        {
            foreach (GameObject gam in createdItems)
            {
                Destroy(gam);
            }
        }
        createdItems.Clear();
    }
}
