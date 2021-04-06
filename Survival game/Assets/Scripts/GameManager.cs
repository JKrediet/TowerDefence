using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gold, crystals;
    public GameObject towerStatsPanel, selectedItem, itemPrefab, shopPanel, inventoryPanel;
    public Transform inventoryParent;
    public TextMeshProUGUI goldText, crystalText, coreHealth, tower_damage, tower_attackSpeed, tower_rotationSpeed, tower_range, tower_projectileSpeed, tower_dps, tower_projectileCount, tower_critChance, tower_critDamage, tower_chain;
    private COREHealth core;
    private Towers towers;
    private Shop coffeShop;
    private Inventory playerInventory;
    public List<GameObject> towerItemPosition, playerItems;
    private bool suckadick;

    private void Start()
    {
        core = FindObjectOfType<COREHealth>();
        playerInventory = FindObjectOfType<Inventory>();
    }

    //basic text stuff!!!!
    private void Update()
    {
        goldText.text = "Gold: " + gold.ToString("F0");
        crystalText.text = "Crystals: " + crystals.ToString("F0");
        coreHealth.text = "CORE: " + core.Health.ToString("F0");
    }
    #region recieve currency
    public void GiveGold(float amount)
    {
        StartCoroutine(GoldOverTime(amount));
    }
    public void GiveCrystals(float amount)
    {
        StartCoroutine(CrystalOverTime(amount));
    }
    private IEnumerator GoldOverTime(float amount)
    {
        for(int i = 0; i < amount; i++)
        {
            gold++;
            
            float tempGold = amount;
            tempGold--;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator CrystalOverTime(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            crystals++;
            float tempCrystal = amount;
            tempCrystal--;
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion //<-- region

    //hier doet inventory openen
    public void TowerStats(Transform selected) // en hier
    { 
        towerStatsPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        towers = selected.GetComponentInChildren<Towers>();
        //give tower to camera!
        Zoom(towers.transform);

        //set items for inventory
        playerItems = new List<GameObject>(playerInventory.itemsInInventory);
        if (!suckadick)
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                GameObject item = Instantiate(playerItems[i], inventoryParent);
                playerItems[i] = item;
            }
            suckadick = true;
        }
        for (int i = 0; i < towers.items.Count; i++)
        {
            towers.items[i].SetActive(true);
        }
        SetTowerStats();
        ReOrderItemLists();
    }
    public void MakeTowerItems(GameObject _oldItem, int _itemNumber)
    {
        //make empty item
        GameObject newItem = Instantiate(itemPrefab);
        
        //give stats
        ItemValue oldItem_values = _oldItem.GetComponent<ItemValue>();
        newItem.GetComponent<ItemValue>().damage = oldItem_values.damage;
        newItem.GetComponent<ItemValue>().attackSpeed = oldItem_values.attackSpeed;
        newItem.GetComponent<ItemValue>().rotationSpeed = oldItem_values.rotationSpeed;
        newItem.GetComponent<ItemValue>().range = oldItem_values.range;
        newItem.GetComponent<ItemValue>().bulletSpeed = oldItem_values.bulletSpeed;
        newItem.GetComponent<ItemValue>().projectileCount = oldItem_values.projectileCount;
        newItem.GetComponent<ItemValue>().critChance = oldItem_values.critChance;
        newItem.GetComponent<ItemValue>().critDamage = oldItem_values.critDamage;
        newItem.GetComponent<ItemValue>().chain = oldItem_values.chain;
        newItem.GetComponent<ItemValue>().element = oldItem_values.element;

        //remove old one
        Destroy(_oldItem); //<-- inportant, no more further use

        //give pos
        newItem.GetComponent<ItemValue>().numberInInventory = _itemNumber;
        newItem.transform.position = towerItemPosition[_itemNumber].transform.position;
        newItem.transform.rotation = towerItemPosition[_itemNumber].transform.rotation;
        newItem.transform.SetParent(towerItemPosition[_itemNumber].transform);
        newItem.transform.localScale = new Vector3(1, 1, 1);

        //give current state
        newItem.GetComponent<ItemValue>().ChangeButtonText(1);

        //give item to tower
        towers.items.Add(newItem);

        //update stats
        towers.UpdateItemStats();
        SetTowerStats();
    }
    public void MakeInvertoryItems(GameObject _oldItem, int _itemNumber)
    {
        //make empty item
        GameObject newItem = Instantiate(itemPrefab);

        //give stats
        ItemValue oldItem_values = _oldItem.GetComponent<ItemValue>();
        newItem.GetComponent<ItemValue>().damage = oldItem_values.damage;
        newItem.GetComponent<ItemValue>().attackSpeed = oldItem_values.attackSpeed;
        newItem.GetComponent<ItemValue>().rotationSpeed = oldItem_values.rotationSpeed;
        newItem.GetComponent<ItemValue>().range = oldItem_values.range;
        newItem.GetComponent<ItemValue>().bulletSpeed = oldItem_values.bulletSpeed;
        newItem.GetComponent<ItemValue>().projectileCount = oldItem_values.projectileCount;
        newItem.GetComponent<ItemValue>().critChance = oldItem_values.critChance;
        newItem.GetComponent<ItemValue>().critDamage = oldItem_values.critDamage;
        newItem.GetComponent<ItemValue>().chain = oldItem_values.chain;
        newItem.GetComponent<ItemValue>().element = oldItem_values.element;

        //remove old one
        if (oldItem_values.isHoldBy == 1)
        {
            if (_itemNumber == 0)
            {
                towers.item1 = false;
                towers.elementSlot1 = 0;
            }
            else if (_itemNumber == 1)
            {
                towers.item2 = false;
                towers.elementSlot2 = 0;
            }
            else if (_itemNumber == 2)
            {
                towers.item3 = false;
                towers.elementSlot3 = 0;
            }
            towers.items.Remove(_oldItem);
            towers.UpdateItemStats();
            SetTowerStats();
        }
        Destroy(_oldItem); //<-- inportant, no more further use

        //give position
        newItem.transform.SetParent(inventoryParent);
        newItem.transform.localScale = new Vector3(1, 1, 1);
        newItem.transform.position = inventoryParent.transform.position;
        newItem.transform.rotation = inventoryParent.transform.rotation;

        //give current state
        newItem.GetComponent<ItemValue>().ChangeButtonText(0);

        //give item to player
        playerItems.Add(newItem);
    }
    public void SetTowerStats()
    {
        //set tower stats
        tower_damage.text = $"Damage: " + towers.damage.ToString("F0");
        tower_attackSpeed.text = $"Attack speed: " + towers.attackSpeed.ToString("F0") + "/sec";
        tower_rotationSpeed.text = $"Rotation speed: " + towers.rotationSpeed.ToString("F0") + "°/sec";
        tower_range.text = $"Range: " + towers.towerRange.ToString("F0");
        tower_projectileSpeed.text = $"Projectile speed: " + towers.bulletSpeed.ToString("F0");
        tower_projectileCount.text = $"Projectile count: " + towers.projectileCount.ToString("F0");
        tower_critChance.text = $"Crit chance: {towers.critChance.ToString("F0")}%";
        tower_critDamage.text = $"Crit damage: {towers.critDamage.ToString("F0")}%";
        tower_chain.text = $"Chain amount: {towers.chain.ToString("F0")}";
        tower_dps.text = $"DPS: {towers.damage * towers.attackSpeed + (towers.critChance * 0.01f * towers.critDamage)}";
    }
    public void DeSelectTower() // hier
    {
        if(towers != null)
        {
            FindObjectOfType<CameraFollow>().ZoomInOnTower(towers.transform);
            playerItems.RemoveAll(x => x == null);
            DeSelectItem();
            for (int i = 0; i < towers.items.Count; i++)
            {
                towers.items[i].SetActive(false);
            }
            playerInventory.itemsInInventory = new List<GameObject>(playerItems);
            playerItems.Clear();
            towerStatsPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            towers = null;
        }
        else if(coffeShop != null)
        {
            shopPanel.SetActive(false);
            Zoom(coffeShop.transform);
            inventoryPanel.SetActive(false);
        }
    }
    public void SelectItem(GameObject item)
    {
        playerItems.RemoveAll(x => x == null);
        DeSelectItem();
        ReOrderItemLists();
        selectedItem = item;
        selectedItem.GetComponent<ItemValue>().TurnUiOn(true);
    }
    public void GiveItemToTower()
    {
        if(towers.item1 == false)
        {
            MakeTowerItems(selectedItem, 0);
            towers.item1 = true;
        }
        else if(towers.item2 == false)
        {
            MakeTowerItems(selectedItem, 1);
            towers.item2 = true;
        }
        else if(towers.item3 == false)
        {
            MakeTowerItems(selectedItem, 2);
            towers.item3 = true;
        }
    }
    public void GiveItemToPlayer()
    {
        MakeInvertoryItems(selectedItem, selectedItem.GetComponent<ItemValue>().numberInInventory);
    }
    private void ReOrderItemLists()
    {
        for(int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].GetComponent<ItemValue>().numberInInventory = i;
        }
    }
    public void DeSelectItem()
    {
        if (selectedItem)
        {
            selectedItem.GetComponent<ItemValue>().TurnUiOn(false);
            selectedItem = null;
        }
    }

    //shop
    public void SelectShop(Transform selected)
    {
        shopPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        coffeShop = selected.GetComponent<Shop>();
        FindObjectOfType<CameraFollow>().ZoomInOnTower(coffeShop.transform);
    }
    public void BuyItemFromShop()
    {
        if(gold > selectedItem.GetComponent<ItemValue>().goldCost)
        {
            gold -= selectedItem.GetComponent<ItemValue>().goldCost;
            selectedItem.GetComponent<ItemValue>().goldCost *= 0.7f;
            selectedItem.GetComponent<ItemValue>()._GiveItem();
        }
    }
    public void SellTower()
    {
        gold += FindObjectOfType<PlayerController>().towerCost * 0.7f;

        for (int i = 0; i < 3; i++)
        {
            if (towers.items.Count > 0)
            {
                SelectItem(towers.items[0]);
                GiveItemToPlayer();
                DeSelectItem();
            }
        }
        playerItems.RemoveAll(x => x == null);
        GameObject temp = towers.transform.parent.gameObject;
        FindObjectOfType<PlayerController>().TurnOffUi();
        Destroy(temp);
        towers = null;
    }
    public void Zoom(Transform trans)
    {
        FindObjectOfType<CameraFollow>().ZoomInOnTower(trans);
    }
}
