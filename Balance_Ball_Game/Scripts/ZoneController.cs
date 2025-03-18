using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneController : ZoneBase
{
    [SerializeField] private int maxCircles = 3;
    [SerializeField] private Transform setTransform;
    [SerializeField] private List<GameObject> circles = new List<GameObject>();
    private GameObject aboveLimitBall;
    public event Action OnZoneChanged;


    void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Circle"))
        {
            CircleBall circle = collision.gameObject.GetComponent<CircleBall>();
            if (circle != null && circle.isHandled) return;
                        
            HandleCircleCollision(collision.gameObject);
          
        }
    }
    private void HandleCircleCollision(GameObject circle)
    {
        if (AddCircle(circle))
        {
            CircleBall circleBehavior = circle.GetComponent<CircleBall>();
            circleBehavior.SetStaticState();
        }
        else
        {
            aboveLimitBall = circle;
            OnZoneChanged?.Invoke();
            Destroy(circle.gameObject);
        }
    }
    
    public GameObject GetAboveLimitBall()
    {
        return aboveLimitBall;
    }

    public override bool AddCircle(GameObject circle)
    {   
        if (circle == null || circle.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("Circle is invalid!");
            return false;
        }
        if (circles.Count >= maxCircles) 
        {
            return false;
        }
        SetInBoxZone(circle);

        OnZoneChanged?.Invoke(); // Уведомляем об изменении
        return true;
    }
    private void SetInBoxZone(GameObject circle)
    {
        circle.transform.SetParent(setTransform);
        float yOffset = circles.Count * 1.8f; // Уменьшите множитель до 0.5f
        circle.transform.localPosition = new Vector2(0, yOffset);
        circles.Add(circle);        
    }
    IEnumerator RearrangeCircles()
    {
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < circles.Count; i++)
        {           
            circles[i].transform.localPosition = Vector2.up * i * 1.8f;
        }
    }
    
    public override void ClearAll()
    {
        foreach (var circle in circles)
        {
            Destroy(circle);
        } 
        circles.Clear();
        OnZoneChanged?.Invoke();
    }          
   
    public override Color? GetColor()
    {
        if (circles.Count == 0) return null;
        return circles[0].GetComponent<SpriteRenderer>().color;
    }

    public override Vector2[] GetCirclePositions()
    {
        return circles
        .Where(c => c != null)
        .Select(c => (Vector2)c.transform.position)
        .ToArray();
    }
    public SpriteRenderer GetCircleAtPosition(int index)
    {
        if (index < 0 || index >= circles.Count) return null;
        return circles[index].GetComponent<SpriteRenderer>();
    }

    public void RemoveCircleAt(int index)
    {   
        if (index < 0 || index >= circles.Count) return;
        
        Destroy(circles[index]);
        circles.RemoveAt(index);        
        StartCoroutine(RearrangeCircles());
        OnZoneChanged?.Invoke();
    }
    public bool IsFull() => circles.Count >= maxCircles;

    public bool IsColorMatch()
    {
        if (circles.Count == 0) return false;
        foreach (var circle in circles)
        {
            if (circle == null || circle.GetComponent<SpriteRenderer>() == null)
                return false;
        }
        Color firstColor = circles[0].GetComponent<SpriteRenderer>().color;
        return circles.All(c => c.GetComponent<SpriteRenderer>().color == firstColor);
    }


}