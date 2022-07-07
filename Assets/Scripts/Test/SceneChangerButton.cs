using System;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Test
{
    public class SceneChangerButton : Button
    {
        public event Action ButtonClick;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            onClick.AddListener(InvokeWithDelay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onClick.RemoveListener(InvokeWithDelay);
        }
        
        private async void InvokeWithDelay()
        {
            var delayInSeconds = 2;
            await Task.Delay(delayInSeconds * 1000);
            ButtonClick?.Invoke();
        }
    }
}