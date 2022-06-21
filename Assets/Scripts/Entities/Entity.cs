using System.Collections;
using Interfaces;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{ 
    protected abstract EntityStats Stats { get; }
    
}

