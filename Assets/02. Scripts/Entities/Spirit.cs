using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpiritState
{

}

public class Spirit : Entity
{
    public SpiritState spiritState { get; private set; }
    

    protected override void OnEntityDied()
    {
    }

    protected override void OnTakeDamage(Entity caster, float dmg)
    {
    }

    protected override void UpdateEntity()
    {
    }
}
