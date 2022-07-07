using System;
using System.Collections.Generic;

namespace Events
{
    public class FlowEventFactory
    {
        public static int GameFlowCount = 0;
        private static List<GameFlowEvent> _listEvents = new List<GameFlowEvent>();
        public static GameFlowEvent CreatEvent(string EventName ,Action completed ,float loadingWeight)
        {
            GameFlowEvent gameFlowEvent = new GameFlowEvent(EventName ,completed ,GameFlowCount++ ,loadingWeight);
            _listEvents.Add(gameFlowEvent);
            return gameFlowEvent;
        }

        public static List<GameFlowEvent> GetAllGameFlow()
        {
            return _listEvents;
        }

        /// <summary>
        /// 得到GameFlow
        /// </summary>
        /// <param name="index">index是从0开始的</param>
        public static GameFlowEvent Get(int index)
        {
            return _listEvents[index];
        }
    }
}