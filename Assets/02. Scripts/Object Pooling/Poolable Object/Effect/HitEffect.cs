public class HitEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.HitParticle;
    }

    public override void OnActivate()
    {
        base.OnActivate();
        transform.localScale = UnityEngine.Vector3.one * 2;
    }
}
