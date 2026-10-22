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
        
        private void Awake()
        {
            grabbable = GetComponent<Grabbable>();
            interactables = GetComponentsInChildren<HandGrabInteractable>();
                
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
            var interactors = interactables.ToArray().SelectMany(interactable => interactable.SelectingInteractors);
            
            foreach (var interactor in interactors)
            {
                interactor.ForceRelease();
            }
        }

        public void DisableGrab()
        {
            foreach (var interactable in interactables)
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
        }
    }
}