using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PunchingBallController : MonoBehaviour
{

    private bool _touched = false;

    public void test()
    {
        Debug.Log("yo");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_touched)
        {
            // m_rigidbody.velocity = new Vector2(3, m_rigidbody.velocity.y);

        }
    }
}
