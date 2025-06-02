using System;
using System.Collections.Generic;
using Trackables;
using Trackables.Items;
using Utils;
using Utils.SimpleDI;
using uo = Utils.Observable;

namespace Runtime
{
    [Serializable]
    public class TrackableRegistrar
    {
        public void Register(ServiceProvider serviceProvider)
        {
            //Tracking active metal objects. Used by metal detector.
            //Ideally, it is worth replacing collections with a separate class.
            var metallicTrackable = new ActiveTrackableState<MetallicTrackableAbstract>();
            serviceProvider
                .Register<uo::IObservable<IReadOnlyCollection<MetallicTrackableAbstract>>>(metallicTrackable);
            serviceProvider.Register<IWriteOnlyCollection<MetallicTrackableAbstract>>(metallicTrackable);
        }
    }
}