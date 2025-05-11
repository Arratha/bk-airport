using System;
using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items
{
    public class ItemStorage : MonoBehaviour
    {
        public IReadOnlyCollection<ItemIdentifier> items => selfItems;
        [SerializeField] private List<ItemIdentifier> selfItems;

        public event Action<ItemIdentifier> OnItemAdded;
        public event Action<ItemIdentifier> OnItemRemoved;

        public void AddItem(ItemIdentifier identifier)
        {
            selfItems.Add(identifier);
            OnItemAdded?.Invoke(identifier);
        }

        public void RemoveItem(ItemIdentifier identifier)
        {
            selfItems.Remove(identifier);
            OnItemRemoved?.Invoke(identifier);
        }
    }
}