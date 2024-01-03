using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region public variables

    public float speed = 3f;

    #endregion

    #region private variables

    private Vector3 inputVector;

    #endregion

    private Rigidbody rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");        
    }

    private void FixedUpdate()
    {
        Vector3 nextPosition = inputVector.normalized * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextPosition);

        rigid.velocity = Vector3.zero;
    }
}
