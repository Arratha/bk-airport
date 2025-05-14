using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Player
{
    public class Teleport : MonoBehaviour
    {
        public void Invoke()
        {
            var state = ServiceProvider.instance.Resolve<IObservableState<TeleportInfo>>();
            state.HandleUpdate(new TeleportInfo
            {
                position = transform.position,
                rotation = transform.eulerAngles
            });
        }
    }
}