using System.Threading.Tasks;
using Arr.EventsSystem;
using Arr.ModulesSystem;
using UnityEngine;

namespace Arr.ViewModuleSystem
{
    public class ViewModule<T> : BaseModule,
        IEventListener<EventOpenView<T>>,
        IEventListener<EventCloseView<T>>
    where T : View
    {
        protected T view;

        protected readonly string viewName = typeof(T).Name.ToLower();

        protected override async Task OnInitialize()
        {
            var prefab = ViewPrefabDatabase.Get(viewName);
            view = Object.Instantiate(prefab).GetComponent<T>();
            view.gameObject.SetActive(view.ActiveOnSpawn);
        }

        protected override async Task OnLoad()
        {
            await view.Load();
        }

        protected override async Task OnUnload()
        {
            await view.Unload();
            Object.Destroy(view.gameObject);
        }

        public void OnEvent(EventOpenView<T> data)
        {
            view.Open();
            OnOpen();
        }

        public void OnEvent(EventCloseView<T> data)
        {
            view.Close();
            OnClose();
        }
        
        protected virtual void OnOpen(){}
        protected virtual void OnClose(){}
    }
}