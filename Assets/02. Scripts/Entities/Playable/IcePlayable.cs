using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlayable : PlayableCtrl
{
    [SerializeField] Material freezeMaterial;
    protected override void OnEntityDied()
    {
    }

    protected override void PlayerSkill()
    {
        ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.FreezeCircle, transform.position);
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("ENEMY") | 1 << LayerMask.NameToLayer("BOSS"));
        if (enemies.Length > 0)
        {
            foreach( var enemy in enemies )
            {
                Entity target = enemy.GetComponent<Entity>();
                target.AddEffect(new Freeze(freezeMaterial, 1, 6f, target));
            }
        }
    }
}
