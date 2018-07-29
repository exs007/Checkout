using System;
using System.Linq;
using System.Collections.Generic;
using CheckoutKataAPI.Constants;

namespace CheckoutKataAPI.Workflow.Base
{
    /// <summary>
    /// List based execution processor. A specific list of actions should be specified for a concrete processor in a derifed class
    /// </summary>
    /// <typeparam name="T">WorkflowDataContext</typeparam>
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
            var processorContext = new WorkflowProcessorContext(_provider);
            //go through actions with an executing context and a custom data context
            foreach (var action in Actions)
            {
                var workflowAction = (IWorkflowAction<T>)Activator.CreateInstance(action.ActionType);
                workflowAction.ExecuteAction(context, processorContext);
            }

            return context;
        }

        //Setup an action into a pipeline.
        protected void SetupAction<A>() where A: IWorkflowAction<T>
        {
            var actionItem = new ActionItem(typeof(A));
            SetupAction(actionItem);
        }

        private void SetupAction(ActionItem actionItem)
        {
            var exist = Actions.Any(p => p.ActionType.Equals(actionItem.ActionType));
            if (exist)
            {
                throw new ArgumentException(MessageConstants.LIST_BASED_WORKFLOW_PROCESSOR_NOT_ALLOW_SAME_ACTION_MULTIPLE_TIME);
            }
            Actions.Add(actionItem);
        }
    }
}
