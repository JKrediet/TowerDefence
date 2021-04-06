using UnityEngine;

public class Fire : MonoBehaviour
{
    public float attackSpeed, bulletSpeed, gunDamage;

    public LayerMask mask;
    public Rigidbody bullet;
    //privates 
    private float nextFire, attackTime;

    private void Update()
    {
        transform.rotation = PlayerRotation();
    }
    public void FireBullet()
    {
        if(Time.time >= nextFire)
        {
            //first* line
            attackTime = attackSpeed / (attackSpeed * attackSpeed);
            nextFire = attackTime + Time.time;
            Rigidbody clone = Instantiate(bullet, transform.position, transform.rotation);
            clone.velocity = clone.transform.forward * bulletSpeed;
            clone.GetComponent<BulletDamage>().damage = gunDamage;
            clone.GetComponent<BulletDamage>().shooter = gameObject;
        }
    }
    public Quaternion PlayerRotation()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos;
        Quaternion lookRotation;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            pos = (hit.point - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
        }
        else
        {
            pos = (hit.point - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
        }
        return lookRotation;
    }
}
