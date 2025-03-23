using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : PlayerComponent
{
    [SerializeField] private CinemachineVirtualCamera normalModCamera;
    [SerializeField] private CinemachineVirtualCamera dialogModCamera;
    [SerializeField] private CinemachineVirtualCamera zoomModCamera;

    void Start()
    {
        if(Object.HasInputAuthority)
            SwitchCamera(normalModCamera);
    }
    private void SwitchCamera(CinemachineVirtualCamera targetCamera)
    {
        // 모든 카메라의 우선순위를 낮춤
        normalModCamera.Priority = 10;
        dialogModCamera.Priority = 10;
        zoomModCamera.Priority = 10;

        // 타겟 카메라의 우선순위를 높임
        if (targetCamera != null)
        {
            targetCamera.Priority = 20;
        }
    }
}
