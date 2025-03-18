using System;
using UnityEngine;

public interface IComboChecker
{
    void CheckCombos();
    event Action<Color> OnComboDetected;
}