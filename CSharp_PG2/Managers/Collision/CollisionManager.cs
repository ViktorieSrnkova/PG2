using System;
using System.Collections.Generic;

namespace CSharp_PG2.Managers.Collision;

public class CollisionManager
{
    
    public const float gravityAcceleration = 9.81f;
    public const float airDrag = 0.1f;
    
    public void Run(float deltaTime, List<Figure> figures)
    {
        var orderedFigures = OrderFiguresByAltitude(figures);
        foreach (var figure in orderedFigures)
        {
            Console.WriteLine($"{figure.GetName()}: {figure.Position.Y}");    
        }
    }
    
    private List<Figure> OrderFiguresByAltitude(List<Figure> figures)
    {
        var sortedList = new List<Figure>(figures);
        
        sortedList.Sort((figure1, figure2) => figure1.Position.Y.CompareTo(figure2.Position.Y));

        return sortedList;
    }
}