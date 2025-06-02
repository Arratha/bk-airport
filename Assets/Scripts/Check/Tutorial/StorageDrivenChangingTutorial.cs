using System.Collections.Generic;
using System.Linq;
using Check.AdditionalCheck;
using Items.Base;
using Items.Storages;
using UnityEngine;
using Utils.Observable;

namespace Check.Tutorial
{
    public class StorageDrivenChangingTutorial : ChangingTutorialAbstract
    {
        [SerializeField] private ProcessedPassengerHandler handler;
        [SerializeField] private GameObject storageParent;
       [SerializeField] private List<StorageAbstract> _storages = new();

        private IObservableState<CheckType> _state;
        
        protected override void HandleInit()
        {
            handler.OnSetPassenger += HandleSetPassenger;
        }
        
        private void HandleSetPassenger()
        {
            IsHidden = false;
         
            UpdateStorages();
            SetText();
        }
        
        private void HandleItemRemoved(ItemIdentifier _)
        {
            IsHidden = true;

            if (!WasShown)
            {
                WasShown = true;
                TutorialState.HandleUpdate(stage + 1);
            }
            
            SetText();
        }

        private void UpdateStorages()
        {
            _storages.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnItemRemoved -= HandleItemRemoved;
                }
            });
            
            _storages = storageParent.GetComponentsInChildren<StorageAbstract>(false).ToList();
            _storages.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnItemRemoved += HandleItemRemoved;
                }
            });
        }
        
        private void OnDisable()
        {
            _storages.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnItemRemoved -= HandleItemRemoved;
                }
            });
        }

        protected override void HandleDestroy()
        {
            handler.OnSetPassenger -= HandleSetPassenger;
        }
    }
}