using Stardust;
using System.Collections.Generic;

public class DialogSystem : GameSystem, IUpdate
{
    List<IComponent> _componentList = new List<IComponent>();

    public void Init()
    {
        _componentList.Add(engine.CheckComponent<DialogData>(this));
        _componentList.Add(engine.CheckComponent<DialogAdapter>(this));
        engine.LinkDependences();

        foreach (var item in _componentList)
        {
            var u = item as IInitialize;
            if (u != null)
                u.Init();
        }
    }

    public void OnUpdate(float t)
    {
        foreach (var item in _componentList)
        {
            var u = item as IUpdate;
            if (u != null)
                u.OnUpdate(t);
        }
    }
}
