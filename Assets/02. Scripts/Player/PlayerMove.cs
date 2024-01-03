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
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.z = Input.GetAxisRaw("Vertical");        
    }

    private void FixedUpdate()
    {
        Vector3 velocity = inputVector.normalized * speed;

        rigid.velocity = velocity;
    }
}
