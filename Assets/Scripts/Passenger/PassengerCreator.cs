using System;
using System.Collections.Generic;
using System.Linq;
using Commands.Contexts;
using Items;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Passenger
{
    [Serializable]
    public class PassengerCreator
    {
        [SerializeField] private PassengerController prefab;

        [Space, SerializeField, Range(0, 1)] private float minAccuracy;
        [SerializeField, Range(0, 1)] private float maxAccuracy;

        [Space, SerializeField] private List<ItemsPreset> itemPresets;

        public PassengerController InstantiatePlayer()
        {
            var passenger = Object.Instantiate(prefab);

            var preset = itemPresets[Random.Range(0, itemPresets.Count)];
            var items = preset.items.ToArray();
            passenger.EnqueueCommand(new GetItemsContext(items));

            var accuracy = Random.Range(minAccuracy, maxAccuracy);
            passenger.accuracy = accuracy;

            return passenger;
        }
    }
}