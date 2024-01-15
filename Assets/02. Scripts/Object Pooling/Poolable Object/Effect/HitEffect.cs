public class HitEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.HitParticle;
    }
}
