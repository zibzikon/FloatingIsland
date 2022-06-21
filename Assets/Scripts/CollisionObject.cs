using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class CollisionObject : MonoBehaviour
{
    public Collider Collider { get; private set; }
    
    [SerializeField]
    private Transform _parent;
    public Transform Parent => _parent;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }
}

