using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static event Action OnTap = delegate { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTap?.Invoke();
        }
    }
}