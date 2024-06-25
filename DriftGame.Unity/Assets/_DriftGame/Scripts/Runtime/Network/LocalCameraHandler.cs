using System;
using System.Collections;
using Cinemachine;
using Fusion;
using UnityEngine;

public class LocalCameraHandler : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            SetupVirtualCamera(transform, transform);
        }
    }

    private void SetupVirtualCamera(Transform follow, Transform lookAt)
    {
        _virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        _virtualCamera.Follow = follow;
        _virtualCamera.LookAt = lookAt;

    }
}
