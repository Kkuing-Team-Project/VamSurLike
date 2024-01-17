using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashShooting : Augmentation
{
    

    public SplashShooting(int level, int maxLevel) : base(level, maxLevel)
    {
    }

    protected override AugmentationEventType GetEventType()
    {
        return AugmentationEventType.ON_HIT;
    }

    public override void AugmentationEffect(Entity sender, AugEventArgs e)
    {
        float radius = float.Parse(GameManager.instance.augTable[level]["SplashShooting"].ToString());
        Debug.Log(radius + ", " + level);
        var effect = ObjectPoolManager.Instance.objectPool.GetObject(
            ObjectPool.ObjectType.Explosion,
            e.eventTr.position).GetComponent<ExplosionEffect>();
        effect.SetSize(Vector3.one * radius);
        Collider[] col = Physics.OverlapSphere(e.eventTr.position, radius, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
        foreach (var enemy in col)
        {
            enemy.GetComponent<Entity>().TakeDamage(GameManager.instance.player, GameManager.instance.player.stat.Get(StatType.DAMAGE));
        }
    }
}
