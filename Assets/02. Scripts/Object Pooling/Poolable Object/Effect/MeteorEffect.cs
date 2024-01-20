public class MeteorEffect : EffectParticle
{
    public override void OnCreate()
    {
        base.OnCreate();
        objectType = ObjectPool.ObjectType.Meteor;
        SoundManager.Instance.PlayOneShot("Sound_EF_UP_Meteor");
    }
}
