using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    /// <summary>
    /// Events inside the program.
    /// </summary>
    public enum EVENT { NotificationCreated, ShowTray, HideTray, SceneRebuild, TimerShow, TimerHide };
    public class EventManager
    {
        // Stores the delegates that get called when an event is fired
        private static Dictionary<EVENT, Action> eventTable
                     = new Dictionary<EVENT, Action>();
 
        // Adds a delegate to get called for a specific event
        public static void AddHandler(EVENT evnt, Action action)
        {
            if (!eventTable.ContainsKey(evnt)) eventTable[evnt] = action;
            else eventTable[evnt] += action;
        }

        // Removes a delegate to get called for a specific event
        public static void RemoveHandlers()
        {
           eventTable = new Dictionary<EVENT, Action>();
        }

        // Fires the event
        public static void Broadcast(EVENT evnt)
        {
            try
            {
                if (eventTable.Count != 0 && eventTable[evnt] != null)
                {
                    eventTable[evnt]();
                }
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError(e);
            }
        }
    }
}