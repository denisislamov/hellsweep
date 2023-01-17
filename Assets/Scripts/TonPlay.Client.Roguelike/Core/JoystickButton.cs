using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace TonPlay.Roguelike.Client.Core
{
	public class JoystickButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [InputControl(layout = nameof(Vector2))]
        [SerializeField]
        private string _controlPath;

        [SerializeField]
        private RectTransform _holder;

        [SerializeField]
        private RectTransform _targetRect;

        [SerializeField]
        private RectTransform _area;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        private Vector3 _startPos;

        protected void Awake()
        {
            _startPos = _holder.anchoredPosition;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            var cam = eventData.pressEventCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_area, eventData.position, cam, out var holderInAreaPosition);
            _holder.localPosition = holderInAreaPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(_area, eventData.position))
            {
                OnPointerUp(eventData);
                return;
            }

            var holderHalfSize = _holder.rect.width / 2f;

            MoveHandle(eventData.position, eventData.pressEventCamera, holderHalfSize, out var isMovementOutOfHolderBound, out var movementDelta);

            UpdateInputControl(movementDelta);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _holder.anchoredPosition = _startPos;
            _targetRect.anchoredPosition = Vector2.zero;
            SendValueToControl(Vector2.zero);
        }

        private void UpdateInputControl(Vector2 handleMovementDelta)
        {
            var newPos = new Vector2(-handleMovementDelta.x, -handleMovementDelta.y);
            SendValueToControl(newPos);
        }

        private void MoveHandle(
            Vector2 screenPos,
            Camera eventCamera,
            float maxDistance,
            out bool isMovementOutOfHolderBound,
            out Vector2 movementDelta)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_holder, screenPos, eventCamera, out var pointerPositionInHandleAnchor);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_holder,
                                                                    _holder.position,
                                                                    eventCamera,
                                                                    out var holderPositionInHandleAnchor);

            var distance = Vector2.Distance(holderPositionInHandleAnchor, pointerPositionInHandleAnchor);
            var clampedDistance = Mathf.Clamp(distance, 0, maxDistance);
            var direction = (pointerPositionInHandleAnchor - holderPositionInHandleAnchor).normalized;

            var newTargetPosition = direction * clampedDistance;
            movementDelta = holderPositionInHandleAnchor - newTargetPosition / maxDistance;
            isMovementOutOfHolderBound = distance > maxDistance;

            _targetRect.anchoredPosition = newTargetPosition;
        }
    }
}