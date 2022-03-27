using UnityEngine;

public class HealBullet : HomingBullet
{
    [HideInInspector] public int heal;


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == shotPoint.GetComponentInParent<Transform>().gameObject.layer)
            Collision(other);
    }

    protected override void Collision(Collider2D other)
    {
        if (other.TryGetComponent<Creature>(out Creature creature) && creature.bodyCenter == target)
        {
            creature.health += heal;
            if (deathEffect != null) Instantiate(deathEffect, creature.bodyCenter.transform);
            Destroy(gameObject);
        }
    }
}
