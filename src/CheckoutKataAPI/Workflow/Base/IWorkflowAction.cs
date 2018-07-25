namespace CheckoutKataAPI.Workflow.Base
{
    public interface IWorkflowAction<T> where T: WorkflowDataContext
    {
        void ExecuteAction(T context, IWorkflowProcessorContext processorContext);
    }
}
