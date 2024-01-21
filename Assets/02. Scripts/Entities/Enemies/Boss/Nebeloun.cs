using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Nebeloun : MonoBehaviour
{
    public float attackInterval;
    public float moveTime;
    public float attackWaitTime;
    public float attackTime;
    public int attackCnt;

    private PlayableCtrl player;
    private CinemachineImpulseSource cameraShakeSource;
    private SkinnedMeshRenderer skin;
    private Animator animator;

    private void Start()
    {
        player = GameManager.instance.player;
        cameraShakeSource = gameObject.GetComponent<CinemachineImpulseSource>();
        skin = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = gameObject.GetComponentInChildren<Animator>();
        skin.enabled = false;

        StartCoroutine(NebelounAttack(attackInterval));
    }

    public IEnumerator NebelounAttack(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return StartCoroutine(AttackCor(moveTime, attackWaitTime, attackTime, attackCnt));
        }
    }


    public IEnumerator AttackCor(float moveTime, float waitTime, float attackTime, int attackCnt)
    {
        skin.enabled = true;
        animator.SetTrigger("Idle");
        SoundManager.Instance.PlayOneShot("AvatarOfDarkness_MoveAtk01_02");
        transform.rotation = Quaternion.Euler(0, -90, 0);
        cameraShakeSource.GenerateImpulse();
        Vector3 origin = player.transform.position + Vector3.right * 60 + Vector3.up * 40 + Vector3.back * 17;
        Vector3 moveTo = player.transform.position + Vector3.left * 60 + Vector3.up * 40 + Vector3.back * 17;

        for (float elapsedTime = 0; elapsedTime < moveTime; elapsedTime += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(origin, moveTo, elapsedTime / moveTime);
            yield return null;  
        }

        skin.enabled = false;
        yield return new WaitForSeconds(waitTime);

        Vector3 startAttackPos = player.transform.position + Vector3.right * 14;
        Vector3 endAttackPos = player.transform.position + Vector3.left * 14;

        for (int i = 0; i < attackCnt; i++)
        {
            var pos = Vector3.Lerp(startAttackPos, endAttackPos, i / (attackCnt - 1f));
            StartCoroutine(Explosion(pos, 3, 1));
            yield return new WaitForSeconds(attackTime / (float)attackCnt);
        }
    }

    private IEnumerator Explosion(Vector3 pos, float range, float waitTime)
    {
        var portal = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.Portal, pos).GetComponent<PortalEffect>();
        portal.SetColor(Color.red);
        portal.SetSize(Vector3.one * range);
        portal.transform.eulerAngles = new Vector3(90, 0, 0);
        yield return new WaitForSeconds(waitTime);

        var lightning = ObjectPoolManager.Instance.objectPool.GetObject(ObjectPool.ObjectType.LightningBolt, pos).GetComponent<LightningBoltEffect>();
        if(Physics.OverlapSphere(pos, range, 1 << LayerMask.NameToLayer("PLAYER")).Length > 0 )
        {
            player.TakeDamage(null, 2);
        }

        yield return new WaitForSeconds(0.5f);
        portal.ReturnObject();
        yield return new WaitForSeconds(0.5f);
        lightning.ReturnObject();
    }
}
