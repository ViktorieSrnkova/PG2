namespace CSharp_PG2.Entities;

public interface IEntity : IDrawable, IDisposable
{

    public string GetName();

}