using System;
using System.Linq;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

namespace Hammer.MetaWrappers
{
    [RequireComponent(typeof(Grabbable))]
    public class GrabbableWrapper : MonoBehaviour
    {
        public event Action<GrabbableWrapper> OnGrab;
        public event Action<GrabbableWrapper> OnDrop;
        
        private Grabbable grabbable;
        private HandGrabInteractable[] interactables;
        private DistanceHandGrabInteractable[] distanceInteractables;
        
        private void Awake()
        {
            grabbable = GetComponent<Grabbable>();
            interactables = GetComponentsInChildren<HandGrabInteractable>(true);
            distanceInteractables = GetComponentsInChildren<DistanceHandGrabInteractable>(true);
                
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }

        private void OnDestroy()
        {
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
        }

        private void HandlePointerEvent(PointerEvent @event)
        {
            switch (@event.Type)
            {
                case PointerEventType.Select :
                    OnGrab?.Invoke(this);
                    break;
                
                case PointerEventType.Unselect :
                    OnDrop?.Invoke(this);
                    break;
                
                case PointerEventType.Hover:
                case PointerEventType.Unhover:
                case PointerEventType.Move:
                case PointerEventType.Cancel:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForceDrop()
        {
            var interactors = interactables.SelectMany(interactable => interactable.SelectingInteractors).Distinct().ToArray();
            var distanceInteractors = distanceInteractables.SelectMany(interactable => interactable.SelectingInteractors).Distinct().ToArray();
            
            foreach (var interactor in interactors)
            {
                interactor.ForceRelease();
            }

            foreach (var interactor in distanceInteractors)
            {
                interactor.Unselect();
            }
        }

        public void DisableGrab()
        {
            foreach (var interactable in interactables)
            {
                interactable.Disable();
            }

            foreach (var interactable in distanceInteractables)
            {
                interactable.Disable();
            }
        }
        
        public void EnableGrab()
        {
            foreach (var interactable in interactables)
            {
                interactable.Enable();
            }

            foreach (var interactable in distanceInteractables)
            {
                interactable.Enable();
            }
        }
    }
}