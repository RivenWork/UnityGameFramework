using System.Collections.Generic;
using System.Reflection;

namespace Stardust
{
    public abstract class GameEngine
    {
        public class DependenceInfo
        {
            public IComponent component;
            public List<string> gameSystemList;
        }

        #region GameSystem

        protected Dictionary<string, GameSystem> _gameSystemDic = new Dictionary<string, GameSystem>();

        public T InstallGameSystem<T>() where T : GameSystem
        {
            var type = typeof(T);
            var typeName = type.FullName;
            if (_gameSystemDic.ContainsKey(typeName))
                return _gameSystemDic[typeName] as T;

            var gs = System.Activator.CreateInstance<T>();
            var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var p in properties)
            {
                if (GetType().IsSubclassOf(p.PropertyType))
                {
                    p.SetValue(gs, this);
                    break;
                }
            }

            _gameSystemDic.Add(typeName, gs);
            return gs;
        }

        public void UnInstallGameSystem<T>() where T : GameSystem
        {
            var type = typeof(T);
            var typeName = type.FullName;
            if (!_gameSystemDic.ContainsKey(typeName))
                return;

            var gs = _gameSystemDic[typeName];
            var clearable = gs as IClearable;
            if (clearable != null)
                clearable.ClearAllResources();

            var gsTypeName = gs.GetType().FullName;
            foreach (var item in _componentDic)
            {
                var gsList = item.Value.gameSystemList;
                if (gsList.Contains(gsTypeName))
                    gsList.Remove(gsTypeName);
            }
        }

        public void UnInstallGameSystem<T>(bool needClear) where T : GameSystem
        {
            UnInstallGameSystem<T>();
            ClearComponents();
        }

        public void ClearComponents()
        {
            var cacheList = new List<string>();
            foreach (var item in _componentDic)
            {
                var gsList = item.Value.gameSystemList;
                if (gsList.Count == 0)
                    cacheList.Add(item.Key);
            }

            foreach (var item in cacheList)
            {
                var c = _componentDic[item].component;
                var clearable = c as IClearable;
                if (clearable != null)
                    clearable.ClearAllResources();
                _componentDic.Remove(item);
            }
        }

        #endregion

        #region Component

        protected Dictionary<string, DependenceInfo> _componentDic = new Dictionary<string, DependenceInfo>();

        public T CheckComponent<T>(GameSystem gameSystem) where T : IComponent
        {
            var type = typeof(T);
            var typeName = type.FullName;
            if (gameSystem == null)
                throw new System.NullReferenceException("Argument cannot be null!");
            var gsTypeName = gameSystem.GetType().FullName;
            if (_componentDic.ContainsKey(typeName))
            {
                var list = _componentDic[typeName].gameSystemList;
                var gs = list.Find(x => x == gsTypeName);
                if (gs != null)
                    throw new System.ApplicationException(gsTypeName + " has already checked this component : " + typeName);

                list.Add(gsTypeName);
                return (T)_componentDic[typeName].component;
            }

            var newComponent = System.Activator.CreateInstance<T>();
            _componentDic.Add(typeName, new DependenceInfo()
            {
                component = newComponent,
                gameSystemList = new List<string>() { gsTypeName }
            });
            return newComponent;
        }

        #endregion

        public void LinkDependences()
        {
            foreach (var item in _componentDic)
            {
                var properties = item.Value.component.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    var interfaces = p.PropertyType.GetInterfaces();
                    var exist = false;
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        if (interfaces[i] == typeof(IComponent))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                        continue;

                    var pName = p.PropertyType.FullName;
                    if (_componentDic.ContainsKey(pName))
                        p.SetValue(item.Value.component, _componentDic[pName].component);
                    else
                        throw new System.ApplicationException(pName + " not found in GameEngine! Check it before LinkDependences.");
                }

            }
        }
    }
}
