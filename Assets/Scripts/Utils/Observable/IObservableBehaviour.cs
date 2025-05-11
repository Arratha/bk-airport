using System;
using UnityEngine;

namespace Utils.Observable
{
    public interface IObservableBehaviour<T>
    {
        public GameObject gameObject { get; }

        public event Action<T> OnUpdate;
    }
}