using System.Collections.Generic;
using Interactive.Usables;
using Items.Base;
using Items.Storages.Attachers;
using UnityEngine;

namespace Check.Tutorial
{
    public class ConveyorAdditionalTutorial : TutorialAbstract
    {
        [SerializeField] private AttacherStorageAbstract storage;

        private List<IUsable> _usables = new();

        private bool _wasItemAdded;
        private bool _wasCorrectStage;

        public override void HandleUpdate(TutorialStage message)
        {
            if (message == TutorialStage.None)
            {
                Destroy(gameObject);
                return;
            }

            if (stage == message)
            {
                _wasCorrectStage = true;
            }
            else if (_wasCorrectStage)
            {
                Destroy(gameObject);
                return;
            }

            SetText();
        }

        protected override void HandleInit()
        {
            storage.OnItemObjectAdded += HandleItemAdded;
            storage.OnItemObjectRemoved += HandleItemRemoved;
        }

        private void HandleItemAdded(Item item)
        {
            _wasItemAdded = true;

            SetText();

            var usable = item.GetComponentInChildren<IUsable>(true);

            if (usable == null)
            {
                return;
            }

            _usables.Add(usable);
            usable.OnUsed += HandleUse;
        }

        private void HandleItemRemoved(Item item)
        {
            Destroy(gameObject);
        }

        private void HandleUse()
        {
            Destroy(gameObject);
        }

        private void SetText()
        {
            Label.text = _wasItemAdded && _wasCorrectStage ? tutorialText : string.Empty;
        }

        protected override void HandleDestroy()
        {
            _usables.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnUsed -= HandleUse;
                }
            });

            if (storage != null)
            {
                storage.OnItemObjectAdded -= HandleItemAdded;
                storage.OnItemObjectRemoved -= HandleItemRemoved;
            }
        }
    }
}