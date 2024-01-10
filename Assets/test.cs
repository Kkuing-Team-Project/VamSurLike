using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public bool a;
    public GameObject ee;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (a)
		{
            ee.gameObject.SetActive(true);
        }
		else
		{
            ee.gameObject.SetActive(false);
        }
    }
}
