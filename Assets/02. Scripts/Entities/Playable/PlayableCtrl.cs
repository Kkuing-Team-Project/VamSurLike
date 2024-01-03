using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableCtrl : Entity
{

    public float rotSpeed;

    private Vector3 dir;

    protected override void InitEntity()
    {
        base.InitEntity();
        stat.SetDefault(StatType.MOVE_SPEED, 3);
    }

    protected override void UpdateEntity()
    {
        base.UpdateEntity();

        //Player Move
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        transform.Translate(dir * stat.Get(StatType.MOVE_SPEED) * Time.deltaTime, Space.World);
        //

        //Player Attack
        if(GetNearestEnemy() != null)
        {
            Vector3 targetDir = (GetNearestEnemy().transform.position - transform.position).normalized;
            Vector3 lookAtDir = Vector3.RotateTowards(transform.forward, targetDir, rotSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(lookAtDir);
        }
        //
    }

    protected EnemyCtrl GetNearestEnemy()
    {
        var enemies = Physics.OverlapSphere(transform.position, stat.Get(StatType.ATTACK_DISTANCE), 1 << LayerMask.NameToLayer("ENEMY"));
        if (enemies.Length > 0)
        {
            EnemyCtrl result = enemies[0].GetComponent<EnemyCtrl>();
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, result.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                    result = enemy.GetComponent<EnemyCtrl>();
            }
            return result;
        }
        else return null;
    }
}
