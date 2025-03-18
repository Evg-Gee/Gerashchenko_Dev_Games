using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSceneObj : MonoBehaviour
{
    public Vector2 DeftResolutionVector2 = new Vector2(720, 1280);
    [Range(0f, 1f)] public float WidthOrHeight = 0;

    private Camera compCamera;

    private float initialSize;
    private float targetAspect;

    private float initialFov;
    private float horizontalFov = 120f;

    private void Start()
    {
        compCamera = GetComponent<Camera>();
        initialSize = compCamera.orthographicSize;
        targetAspect = DeftResolutionVector2.x / DeftResolutionVector2.y;
        initialFov = compCamera.fieldOfView;
        horizontalFov = CalculateVerticalFov(initialFov, 1 / targetAspect);
    }

    private void Update()
    {
        if (compCamera.orthographic)
        {
            float consttWidthSize = initialSize * (targetAspect / compCamera.aspect);
            compCamera.orthographicSize = Mathf.Lerp(consttWidthSize, initialSize, WidthOrHeight);
        }
        else
        {
            float consttWidthFov = CalculateVerticalFov(horizontalFov, compCamera.aspect);
            compCamera.fieldOfView = Mathf.Lerp(consttWidthFov, initialFov, WidthOrHeight);
        }
    }

    private float CalculateVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRaders = hFovInDeg * Mathf.Deg2Rad;
        float vFovInRaders = 2 * Mathf.Atan(Mathf.Tan(hFovInRaders / 2) / aspectRatio);
        return vFovInRaders * Mathf.Rad2Deg;
    }
}
