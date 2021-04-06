using UnityEngine;
using TMPro;

public class EnemyHealth : BaseHealth
{
    public float goldAmount = 10, crystalAmount = 1;

    public override void DoDamage(float amount, bool crit, float burnDamage, float burnDuration, float freezeDuration, float freezeSlow, float freezeChance, float lightningDamage, float lightningChainAmount, float lightningRange)
    {
        if (Health > 0)
        {
            //elemental effects
            BurnStuff(burnDuration, burnDamage, crit);
            FreezeStuff(freezeDuration, freezeSlow, freezeChance);

            Health -= amount;
            if (damagePopUp)
            {
                height.x = Random.Range(-0.5f, 0.5f);
                height.z = Random.Range(-0.5f, 0.5f);

                if (crit)
                {
                    var temp = Instantiate(critPopUp, transform.position + height, damagePopUp.transform.rotation);
                    temp.GetComponent<TextMeshPro>().text = amount.ToString("F0");

                    Destroy(temp, 3);
                }
                else
                {
                    var temp = Instantiate(damagePopUp, transform.position + height, damagePopUp.transform.rotation);
                    temp.GetComponent<TextMeshPro>().text = amount.ToString("F0");

                    Destroy(temp, 3);
                }
            }
            if (Health <= 0)
            {
                GiveGold();
                FindObjectOfType<Spawner>().EnemyDied();
                Destroy(gameObject);
            }
        }
    }
    protected virtual void GiveGold()
    {
        FindObjectOfType<GameManager>().GiveGold(goldAmount);
        FindObjectOfType<GameManager>().GiveCrystals(crystalAmount);
    }
}
