using System;
using System.Collections.Generic;
using Trackables;
using Trackables.Items;
using Trackables.Labels;
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
            var metallicTrackable = new ActiveTrackableState<MetallicTrackable>();
            serviceProvider.Register<uo::IObservable<IReadOnlyCollection<MetallicTrackable>>>(metallicTrackable);
            serviceProvider.Register<IWriteOnlyCollection<MetallicTrackable>>(metallicTrackable);

            var labelTrackable = new UpdatableTrackableState<LabelTrackable>();
            serviceProvider.Register<uo::IObservable<IReadOnlyCollection<LabelTrackable>>>(labelTrackable);
            serviceProvider.Register<uo::IObserver<LabelTrackable>>(labelTrackable);
            serviceProvider.Register<IWriteOnlyCollection<LabelTrackable>>(labelTrackable);
        }
    }
}