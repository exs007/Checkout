using System;
using System.Linq;
using System.Collections.Generic;

namespace CheckoutKataAPI.Workflow.Base
{
    //List based execution processor
    //A specific list of actions should be specified for a concrete processor in a derifed class
    public class WorkflowProcessor<T> : IWorkflowProcessor<T>
        where T : WorkflowDataContext
    {
        private readonly IServiceProvider _provider;

        protected ICollection<ActionItem> Actions = new List<ActionItem>();

        public WorkflowProcessor(IServiceProvider provider)
        {
            _provider = provider;
        }

        public WorkflowProcessor(IServiceProvider provider, ICollection<ActionItem> actionItems)
        {
            _provider = provider;
            if (actionItems?.Count > 0)
            {
                foreach (var actionItem in actionItems)
                {
                    SetupAction(actionItem);
                }
            }
        }

        public T Execute(T context)
        {
            throw new NotImplementedException();
        }

        private void SetupAction(ActionItem actionItem)
        {
            var exist = Actions.Any(p => p.ActionType.Equals(actionItem.ActionType));
            if (exist)
            {
                throw new ArgumentException("The same action is specified more than one time");
            }
            Actions.Add(actionItem);
        }
    }
}
