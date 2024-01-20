public class MeteorEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.Meteor;
        SoundManager.Instance.PlaySound("Sound_EF_UP_Meteor", false, true, "METEOR", false, 0.5f, 1f);
    }
}
