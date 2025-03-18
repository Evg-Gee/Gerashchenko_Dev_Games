using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboChecker : MonoBehaviour, IComboChecker
{
    [SerializeField] private ZoneController[] zones;
    [SerializeField] private ParticleSystem comboParticles;

    public event Action<Color> OnComboDetected = delegate { };
    public event Action<Color> OnMinusScore = delegate { };
    
    private void OnEnable()
    {
        foreach (var zone in zones)
        {
            zone.OnZoneChanged += CheckCombos;
        }
    }

    private void OnDisable()
    {
        foreach (var zone in zones)
        {
            zone.OnZoneChanged -= CheckCombos;
        }
    }
    public void CheckCombos()
    {
        DestroyAboveLimitBall();
        CheckHorizontalCombos();
        CheckVerticalCombos();
        CheckDiagonalCombos();
    }
    private void DestroyAboveLimitBall()
    {
        foreach (var zone in zones)
        {
            if (zone.GetAboveLimitBall())
            {
                Color comboColor = zone.GetColor().Value;
                SpawnParticles(zone.GetAboveLimitBall().transform.position);
                
                OnMinusScore?.Invoke(comboColor);
            }
        }
    }
    private void CheckHorizontalCombos()
    {
        foreach (var zone in zones)
            {
                if (zone.IsFull() && zone.IsColorMatch())
                {
                    Vector2[] positions = zone.GetCirclePositions();
                    SpawnParticles(positions);
                    OnComboDetected?.Invoke(zone.GetColor().Value);
                    zone.ClearAll();
                }
            }
    }
    private void CheckDiagonal(SpriteRenderer c1, SpriteRenderer c2, SpriteRenderer c3)
{
    if (c1 == null || c2 == null || c3 == null) return;
    if (c1.color != c2.color || c2.color != c3.color) return;

    Vector2[] positions = { c1.transform.position, c2.transform.position, c3.transform.position };
    SpawnParticles(positions);

    int index1 = c1.transform.GetSiblingIndex();
    int index2 = c2.transform.GetSiblingIndex();
    int index3 = c3.transform.GetSiblingIndex();

    zones[0].RemoveCircleAt(index1);
    zones[1].RemoveCircleAt(index2);
    zones[2].RemoveCircleAt(index3);

    OnComboDetected?.Invoke(c1.color);
}
    private void CheckDiagonalCombos()
    {
        if (zones.Length != 3) return;

        CheckDiagonal(
            zones[0].GetCircleAtPosition(0),
            zones[1].GetCircleAtPosition(1),
            zones[2].GetCircleAtPosition(2)
        );

        CheckDiagonal(
            zones[2].GetCircleAtPosition(0),
            zones[1].GetCircleAtPosition(1),
            zones[0].GetCircleAtPosition(2)
        );
    }
    
    private void CheckVerticalCombos()
    {
        if (zones.Length != 3) return;

        for (int column = 0; column < 3; column++)
        {
            var columnCircles = new List<SpriteRenderer>();

            foreach (var zone in zones)
            {
                SpriteRenderer circle = zone.GetCircleAtPosition(column);
                if (circle != null)
                columnCircles.Add(circle);
            }

            if (columnCircles.Count == 3 && columnCircles.All(c => c.color == columnCircles[0].color))
            {
                Vector2[] positions = columnCircles.Select(c =>(Vector2) c.transform.position).ToArray();
                SpawnParticles(positions);

                foreach (var zone in zones)
                {
                    zone.RemoveCircleAt(column);
                }

                OnComboDetected?.Invoke(columnCircles[0].color);
            }
        }
    }
    private bool CheckSameColor(List<Vector2> positions)
    {
        if (positions.Count == 0)
        {
            return false;
        }
        Color firstColor = GetColor(positions[0]);
        return positions.All(pos => GetColor(pos) == firstColor);
    }
    private Color GetColor(Vector2 position)
    {
        foreach (var zone in zones)
        {
            var positions = zone.GetCirclePositions();
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == position)
                {
                    Color? color = zone.GetColor();
                    return color ?? Color.clear; // Защита от null
                }
            }
        }
        return Color.clear;
    }
        
    private void SpawnParticles(Vector2[] positions)
    {                
        foreach (var pos in positions)
        {
            var particle = Instantiate(comboParticles, pos, Quaternion.identity);
            Destroy(particle.gameObject, 2.5f);     
        }
    }
    private void SpawnParticles(Vector2 position)
    {                
        var particle = Instantiate(comboParticles, position, Quaternion.identity);   
        
        Destroy(particle.gameObject, 2.5f);     
    }
}
