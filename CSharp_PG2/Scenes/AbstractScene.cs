using System.Collections.Generic;
using CSharp_PG2.Entities;
using CSharp_PG2.Managers.Object;

namespace CSharp_PG2.Scenes;

public abstract class AbstractScene
{

    protected readonly List<IEntity> Entities = new List<IEntity>();

}