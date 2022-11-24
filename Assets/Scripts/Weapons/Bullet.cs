using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float damage;

    public void SetDamage(float newDamage)
    {
        this.damage = newDamage;

    }
    public float GetDamage()
    {
        return this.damage;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        takeDamage(hitInfo);
    }

    public virtual void takeDamage(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        Debug.Log(hitInfo.name);
        if (enemy != null)
        {
            enemy.takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
