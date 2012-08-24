using RemixBoard.Core;

namespace RemixBoard.Wpf
{
    public class JobEventHandlerArgs
    {
        public Job Job { get; set; }
        public JobEventHandlerArgs(Job job) {
            Job = job;
        }
    }
}