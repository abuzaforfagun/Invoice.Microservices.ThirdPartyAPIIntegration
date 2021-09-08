using System;

namespace Communication.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceBusQueue : Attribute
    {
        public string Name { get; init; }
        public string DeadLetterReceiver { get; init; }
        public int MaxRetry { get; init; }
        public bool IsScheduleRequired { get; init; }
        public int DifferedTimeInMinutes { get; set; }

        public ServiceBusQueue(
            string name, 
            string deadLetterReceiver = "", 
            int maxRetry = 10, 
            bool isScheduleRequired = false, 
            int differedTimeInMinutes = 30)
        {
            Name = name;
            DeadLetterReceiver = deadLetterReceiver;
            MaxRetry = maxRetry;
            IsScheduleRequired = isScheduleRequired;
            DifferedTimeInMinutes = differedTimeInMinutes;
        }
    }
}
