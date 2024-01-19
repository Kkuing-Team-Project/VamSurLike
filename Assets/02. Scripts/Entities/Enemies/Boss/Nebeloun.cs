using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Nebeloun : MonoBehaviour
{
    public float moveTime;
    public float waitTime;
    public float attackTime;
    public int attackCnt;

    private PlayableCtrl player;
    private CinemachineImpulseSource cameraShakeSource;
    private SkinnedMeshRenderer skin;

    private void Start()
    {
        player = GameManager.instance.player;
        cameraShakeSource = gameObject.GetComponent<CinemachineImpulseSource>();
        skin = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        skin.enabled = false;
        StartCoroutine(AttackCor(moveTime, waitTime, attackTime, attackCnt));
    }


    public IEnumerator AttackCor(float moveTime, float waitTime, float attackTime, int attackCnt)
    {
        skin.enabled = true;
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
        cameraShakeSource.GenerateImpulse();
        yield return new WaitForSeconds(0.5f);
        portal.ReturnObject();
        yield return new WaitForSeconds(0.5f);
        lightning.ReturnObject();
    }
}
