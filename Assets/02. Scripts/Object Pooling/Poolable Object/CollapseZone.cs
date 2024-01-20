using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollapseZone : MonoBehaviour, IPoolable
{
    [Tooltip("�ر��� ��ȭ �ʴ� ������")]
    public float increment;
    [Tooltip("�ر��� ����"), Range(1f, 10f)]
    public float zoneRange;

    public float collapseTime;
    public float elapsedTime { get; private set; }
    public float stablity { get; private set; }
    public ObjectPool pool { get; set; }

    private Spirit spirit;
    private PlayableCtrl player;
    private PortalEffect effect;
    private Spawner spawner;
    public void OnCreate()
    {
        spawner = GameObject.Find("Red Zone Spawner").GetComponent<Spawner>();


    }

    public void OnActivate()
    {
        collapseTime = spawner.stage.waves[spawner.currentWaveIndex].duration;
        Debug.Log(gameObject.name + ": " + collapseTime + ", " + spawner.currentWaveIndex);
        effect = ObjectPoolManager.Instance.objectPool.GetObject(
            ObjectPool.ObjectType.Portal,
            transform.position).GetComponent<PortalEffect>();
        effect.SetSize(Vector3.one * zoneRange);
        effect.SetColor(new Color(47f / 255f, 56f / 255f, 1f));
        effect.transform.eulerAngles = new Vector3(90, 0, 0);
        elapsedTime = 0;
    }

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayableCtrl>();
        spirit = FindObjectOfType<Spirit>();
    }

    private void Update()
    {
        if (spirit == null)
            return;
        if (elapsedTime >= collapseTime)
        {
            Collapse();
            return;
        }

        float value = 0;
        
        if (spirit.spiritState == SpiritState.OCCUPY)
        {
            elapsedTime += Time.deltaTime;
            if (spirit.occupyDelay <= elapsedTime)
            {
                spirit.anim.SetBool("Stabilization", true);
                if ((transform.position - spirit.transform.position).magnitude <= zoneRange)
                    value += increment;
                if (player != null)
                {
                    if ((transform.position - player.transform.position).magnitude <= zoneRange)
                        value += increment;
                }
            }
        }
        stablity = Mathf.Clamp(stablity + (value * Time.deltaTime), 0, 100f);
        if (stablity >= 100)
        {
            spirit.SetBlessType();
            ReturnObject();
        }
    }

    private void Collapse()
    {
        //SoundManager.Instance.PlayOneShot("Sound_EF_CH_Death");
        Debug.LogError("GameOver");
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, zoneRange);
        }
    }
    
    private void OnEnable()
    {        
        spirit.collapseZone = this;
        stablity = 0;
    }
    
    public void ReturnObject()
    {
        spirit.collapseZone = null;
        effect.ReturnObject();
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.CollapseZone);
    }
}
