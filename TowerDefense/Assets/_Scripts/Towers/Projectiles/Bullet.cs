using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Tower parentTower;

    [Header("Bullet Attributes")]
    public Unit unit;
    public float bulletSpeed = 0.5f;
    public Transform target;
    public float damage;


    public virtual void Start(){
        Physics2D.IgnoreLayerCollision(3, 7);
        damage = parentTower.GetDamage();
    }
    public void SetTarget(Transform target){
        this.target = target;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!target) {
            Destroy(gameObject);
            return;
        };

        Vector2 Direction = (target.position - transform.position).normalized;
        rb.velocity = Direction * bulletSpeed;   
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        unit = other.gameObject.GetComponent<Unit>();
        if(unit == null) return;
        if(unit.getHealth() >= damage){
            parentTower.IncreaseDamageDealt(damage);
        } else {
            parentTower.IncreaseDamageDealt(unit.getHealth());
        }
        unit.TakeDamage(damage);
        Destroy(gameObject);       
    }
}
