using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks this game-object as a part to which <see cref="PartDocker"/> can
/// attach.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PartDock : MonoBehaviour
{
    private readonly ISet<PartDocker> dockedParts = new HashSet<PartDocker>();

    public Rigidbody Rigidbody { get; private set; } = null!;

    public void Undock(PartDocker docker)
    {
        dockedParts.Remove(docker);
    }

    public void Dock(PartDocker docker)
    {
        dockedParts.Add(docker);
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
}