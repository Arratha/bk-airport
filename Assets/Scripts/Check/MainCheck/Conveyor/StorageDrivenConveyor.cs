using System.Linq;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Check.MainCheck.Conveyor
{
    //Defines conveyor move condition as any items placed on conveyor
    public class StorageDrivenConveyor : ConveyorAbstract
    {
        // [SerializeField] private StorageAbstract storage;
        //
        // protected override void OnInit()
        // {
        //     ShouldMove = storage.items.Any();
        //     storage.OnItemAdded += HandleItemAdded;
        //     storage.OnItemRemoved += HandleItemRemoved;
        // }
        //
        // private void HandleItemAdded(ItemIdentifier[] identifiers)
        // {
        //     ShouldMove = true;
        // }
        //
        // private void HandleItemRemoved(ItemIdentifier[] identifiers)
        // {
        //     ShouldMove = storage.items.Any();
        // }
    }
}