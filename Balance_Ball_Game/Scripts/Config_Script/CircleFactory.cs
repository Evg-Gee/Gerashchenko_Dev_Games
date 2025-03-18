using UnityEngine;

[CreateAssetMenu(fileName = "CircleFactory", menuName = "Factories/CircleFactory")]
public class CircleFactory : ScriptableObject, ICircleFactory
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private Color[] availableColors;
    
    private Color? forcedColor; // Для принудительного цвета (если нужно)

    public GameObject CreateCircle()
    {
        if (circlePrefab == null)
        {
            Debug.LogError("Prefab not assigned in CircleFactory");
            return null;
        }
        
        GameObject circle = Instantiate(circlePrefab);
        SpriteRenderer renderer = circle.GetComponent<SpriteRenderer>();
        
        // Выбираем цвет: либо принудительный, либо случайный
        Color color = forcedColor ?? availableColors[Random.Range(0, availableColors.Length)];
        renderer.color = color;
        
        // Сбрасываем принудительный цвет
        forcedColor = null;
        return circle;
    }

    public void SetColor(Color color)
    {
        forcedColor = color;
    }
}