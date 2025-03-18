using UnityEngine;

public interface ICircleFactory
{
    GameObject CreateCircle();
    void SetColor(Color color); // Опционально для принудительной установки цвета
}