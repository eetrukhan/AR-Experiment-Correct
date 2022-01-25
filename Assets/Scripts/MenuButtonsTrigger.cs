using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Logic
{
    public class MenuButtonsTrigger : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IGvrPointerHoverHandler
    {
        private long startTime;

        public void OnPointerEnter(PointerEventData eventData)
        {
            startTime = DateTime.Now.Ticks;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            long duration = (long)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;
            if (duration >= GlobalCommon.waitForActionToBeAcceptedPeriod)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
        }

        public void OnGvrPointerHover(PointerEventData eventData)
        {
            long duration = (long)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;
            if (duration >= GlobalCommon.waitForActionToBeAcceptedPeriod)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                startTime = DateTime.Now.Ticks;
            }
        }
    }
}
