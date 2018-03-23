using Stardust;

public class MyEngine : GameEngine
{
    public void Update(float t)
    {
        foreach (var item in _gameSystemDic)
        {
            var u = item.Value as IUpdate;
            if (u != null)
            {
                u.OnUpdate(t);
            }
        }
    }
}
