﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public LayerMask playerLayerMask;
    public float platformTimer;
    public bool isSpecialPlatform;
    public Camera camera;
    public PlayerController player;

    private BoxCollider2D boxCollider2D;
    private Renderer renderer;
    private BoxCollider2D collider;
    private float timeRemaining;
    private bool isPlatformDestroyed;
    private bool hasToDestroyPlatform;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<BoxCollider2D>();
        timeRemaining = platformTimer;
        isPlatformDestroyed = false;
        hasToDestroyPlatform = false;
    }

    void Update()
    {
        if (IsPlayerInPlatform())
        {
            if (isSpecialPlatform)
            {
                camera.GetComponent<Animator>().Play("camera");
                player.isBackwards = true;
                hasToDestroyPlatform = false;
            } else
            {
                hasToDestroyPlatform = true;
            }
            
        }
        HandleBlockDestroy();
        HandleBlockAppear();
    }

    

    private void HandleBlockDestroy()
    {
        if(IsPlayerInPlatform() || hasToDestroyPlatform) 
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                DisablePlatform();
                timeRemaining = platformTimer;
                isPlatformDestroyed = true;
                hasToDestroyPlatform = false;
            }
        }
    }

    private void HandleBlockAppear()
    {
        if(isPlatformDestroyed)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                EnablePlatform();
                timeRemaining = platformTimer;
                isPlatformDestroyed = false;
            }
        }
    }

    private void DisablePlatform()
    {
        renderer.enabled = false;
        collider.enabled = false;
    }

    private void EnablePlatform()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }

    private bool IsPlayerInPlatform()
    {
        Vector2 center = boxCollider2D.bounds.center + new Vector3(0f, 0.2f, 0f); ;
        Vector2 size = boxCollider2D.bounds.size + new Vector3(-0.1f, -0.1f, 0f);
        RaycastHit2D raycast = Physics2D.BoxCast(center, size, 0f, Vector2.up, 0.1f, playerLayerMask);
 
        BoxCast(center, size, 0f, Vector2.up, 0.1f, playerLayerMask);
        return raycast.collider != null;
    }


    static public RaycastHit2D BoxCast(Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);

        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;
    }
}
