using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollapseZone : MonoBehaviour, IPoolable
{
    [Tooltip("ºØ±«Á¸ ÃÊ´ç Áõ°¡°ª")]
    public float increment;
    [Tooltip("ºØ±«Á¸ ¹üÀ§"), Range(1f, 10f)]
    public float zoneRange;
    public float stablity { get; private set; }
    public ObjectPool pool { get; set; }

    private Spirit spirit;
    private PlayableCtrl player;

    public void OnCreate()
    {
    }

    public void OnActivate()
    {
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

        float value = 0;

        if (spirit.spiritState == SpiritState.OCCUPY)
        {
            if ((transform.position - spirit.transform.position).magnitude <= zoneRange)
                value += increment;
            if (player != null)
            {
                if ((transform.position - player.transform.position).magnitude <= zoneRange)
                    value += increment;
            }
        }

        stablity = Mathf.Clamp(stablity + (value * Time.deltaTime), 0, 100f);
        if (stablity >= 100)
        {
            spirit.SetBlessType();
            ReturnObject();
        }
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
        pool.ReturnObject(gameObject, ObjectPool.ObjectType.CollapseZone);
    }
}
