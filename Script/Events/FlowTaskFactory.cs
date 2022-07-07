namespace Events
{
    public class FlowTaskFactory
    {
        public static GameFlowTask CreatTask(CoroutineDelegate coroutineDelegate ,TaskPercentFunc percentageAction)
        {
            GameFlowTask gameFlowTask = new GameFlowTask(coroutineDelegate ,percentageAction);
            return gameFlowTask;
        }
        
        public static GameFlowTask CreatTask(CoroutineDelegate coroutineDelegate)
        {
            GameFlowTask gameFlowTask = new GameFlowTask(coroutineDelegate);
            return gameFlowTask;
        }
        
        public static GameFlowTask CreatTask(TaskPercentFunc percentageAction)
        {
            GameFlowTask gameFlowTask = new GameFlowTask(percentageAction);
            return gameFlowTask;
        }
        
        public static GameFlowTask CreatTask()
        {
            GameFlowTask gameFlowTask = new GameFlowTask();
            return gameFlowTask;
        }

        public static GameFlowTaskGroup CreatTaskGroup()
        {
            GameFlowTaskGroup gameFlowTaskGroup = new GameFlowTaskGroup();
            return gameFlowTaskGroup;
        }
        
        public static GameFlowTaskGroup CreatTaskGroup(int taskNum)
        {
            GameFlowTaskGroup gameFlowTaskGroup = new GameFlowTaskGroup(taskNum);
            return gameFlowTaskGroup;
        }
    }
}