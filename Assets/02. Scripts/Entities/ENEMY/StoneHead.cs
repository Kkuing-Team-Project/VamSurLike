using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHead : EnemyCtrl
{
    public float hp = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
        throw new System.NotImplementedException();
    }

    protected override void EnemyAttack()
    {
        throw new System.NotImplementedException();
    }
}
