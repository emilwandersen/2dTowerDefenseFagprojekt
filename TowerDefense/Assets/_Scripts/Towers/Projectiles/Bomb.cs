using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    [Header("Bomb References")]
    // [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask unitMask;

    // public Tower parentTower
    [SerializeField] private Unit unit;
    private float _range = .5f;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootSound;


    public override void Start()
    {
        base.Start();
        Physics2D.IgnoreLayerCollision(3, 7);
        _damage = _parentTower.GetDamage();
        _audioSource = GetComponent<AudioSource>();
    }

    // public void SetTarget(Transform target){
    //     this.target = target;
    // }
    // Start is called before the first frame update

    // Update is called once per frame
    // private void FixedUpdate()
    // {
    //     if(!target) {
    //         Destroy(gameObject);
    //         return;
    //     };

    //     Vector2 Direction = (target.position - transform.position).normalized;
    //     rb.velocity = Direction * bulletSpeed;   
    // }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        unit = other.gameObject.GetComponent<Unit>();

        //Increase Tower Stats
        IncreaseTotalDamage(unit);
        unit.TakeDamage(_damage);
        _audioSource.PlayOneShot(_shootSound, .8f);
        //Start explosion damage
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _range, (Vector2)transform.position, 0f, unitMask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.tag == "Unit")
            {
                unit = hit.collider.gameObject.GetComponent<Unit>();
                IncreaseTotalDamage(unit);
                unit.TakeDamage(_damage);
                Debug.Log("Hit Unit from Explosion" + hit.collider.gameObject.name);
            }
        }
        Destroy(gameObject);
    }

    void IncreaseTotalDamage(Unit unit)
    {
        if (unit.GetHealth() >= _damage)
        {
            _parentTower.IncreaseDamageDealt(_damage);
        }
        else
        {
            _parentTower.IncreaseDamageDealt(unit.GetHealth());
        }
    }
}


