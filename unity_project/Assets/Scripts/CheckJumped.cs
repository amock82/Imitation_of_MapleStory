﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJumped : MonoBehaviour
{
    public static CheckJumped   instance;
    
    public BoxCollider2D        _col2D;

    private void Awake()
    {
        _col2D = GetComponent<BoxCollider2D>();

        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Player.instance.setIsJumped(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Player.instance.setIsJumped(false);
        }
    }
}
