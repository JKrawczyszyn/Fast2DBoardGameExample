using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPress;

        public bool Pressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPress?.Invoke();

            Pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
        }
    }
}
