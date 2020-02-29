using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    public float startTime;
    public float timer;

    private Renderer renderer;
    private BoxCollider2D collider;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        DecreaseTimer();
    }

    private void DecreaseTimer()
    { 
        timer -= Time.deltaTime;
        Debug.Log(timer);
        if (timer < 0)
        {
            Debug.Log("aaa");
            UpdateState();
            timer = 1f;
        }
    }

    private void UpdateState()
    {
        
        if (renderer.enabled)
        {
            Debug.Log("bbb");
            renderer.enabled = false;
            collider.enabled = false;
        }
        else
        {
            Debug.Log("ccc");
            renderer.enabled = true;
            collider.enabled = true;
        }
    }
}
