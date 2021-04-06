using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemValue : MonoBehaviour
{
    public float damage, attackSpeed, rotationSpeed, range, bulletSpeed, projectileCount, goldCost, critChance, critDamage, chain;
    public int element; //0 = nothing, 1 = lightning, 2 = water, 3 = earth, 4 = ice, 5 = nature, 6 = fire
    //fire = red, lightning = yellow, water = blue, earth = brown, ice = light blue, nature = green, nothing = white
    public Button equip;
    private TextMeshProUGUI buttenText;
    public TextMeshProUGUI itemInfo;
    public int numberInInventory, isHoldBy; // 0 = inventory, 1 = tower, 2 = shop?

    private GameManager gameManager;

    private void Start()
    {
        GetElementColor();
        gameManager = FindObjectOfType<GameManager>();
        buttenText = equip.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void GetElementColor()
    {
        if(element == 0)
        {
            //nothing
            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if(element == 1)
        {
            //lightning
            GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }
        else if (element == 2)
        {
            //water
            GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }
        else if (element == 3)
        {
            //earth
            GetComponent<Image>().color = new Color32(139, 69, 19, 255);
        }
        else if (element == 4)
        {
            //ice
            GetComponent<Image>().color = new Color32(173, 216, 230, 255);
        }
        else if (element == 5)
        {
            //nature
            GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        }
        else if (element == 6)
        {
            //fire
            GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
    }
    public void TurnUiOn(bool value)
    {
        if (isHoldBy == 2)
        {
            FindObjectOfType<Shop>().buy.gameObject.SetActive(value);
            FindObjectOfType<Shop>().cost.gameObject.SetActive(value);
            FindObjectOfType<Shop>().cost.text = $"Cost : {goldCost.ToString("F0")}";
        }
        else
        {
            equip.gameObject.SetActive(value);
        }
        itemInfo.gameObject.SetActive(value);
        itemInfo.text =
              MakeText("Damage: ", new Color32(255, 0, 0, 255), damage)
            + MakeText("AttackSpeed: ", new Color32(124, 136, 197, 255), attackSpeed)
            + MakeText("RotationSpeed: ", new Color32(1, 231, 175, 255), rotationSpeed)
            + MakeText("Range: ", new Color32(19, 87, 193, 255), range)
            + MakeText("ProjectileSpeed: ", new Color32(224, 214, 45, 255), bulletSpeed)
            + MakeText("projectileCount: ", new Color32(130, 59, 254, 255), projectileCount)
            + MakeText("critChance: ", new Color32(222, 11, 233, 255), critChance)
            + MakeText("critDamage: ", new Color32(48, 125, 73, 255), critDamage)
            + MakeText("chainNumber: ", new Color32(206, 104, 95, 255), chain);
    }
    private string MakeText(string valueName, Color color, float value)
    {
        string temp = "";
        if (value > 0)
        {
            temp = $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}> {valueName + value + Environment.NewLine}</color>";
            return temp;
        }
        else
        {
            return temp;
        }
    }
    public void _Selected()
    {
        gameManager.SelectItem(gameObject);
    }
    public void _GiveItem()
    {
        if(isHoldBy == 0)
        {
            gameManager.GiveItemToTower();
        }
        else if(isHoldBy == 1)
        {
            gameManager.GiveItemToPlayer();
        }
        else if(isHoldBy == 2)
        {
            gameManager.GiveItemToPlayer();
            FindObjectOfType<Shop>().cost.text = $"Cost : ";
        }
    }

    //new shiz
    public void ChangeButtonText(int _state)
    {
        isHoldBy = _state;
        buttenText = equip.GetComponentInChildren<TextMeshProUGUI>();
        if (_state == 0)
        {
            buttenText.text = "Equip";
        }
        else if (_state == 1)
        {
            buttenText.text = "Unequip";
        }
    }
}
