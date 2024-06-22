using System;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class LocalCameraHandler : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        if (Object.HasInputAuthority)
        {
            _virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
            SetupVirtualCamera(transform, transform);
        }
    }

    public void SetupVirtualCamera(Transform follow, Transform lookAt)
    {
        _virtualCamera.Follow = follow;
        _virtualCamera.LookAt = lookAt;
    }
}
