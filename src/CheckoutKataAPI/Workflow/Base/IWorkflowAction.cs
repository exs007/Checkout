namespace CheckoutKataAPI.Workflow.Base
{
    /// <summary>
    /// An interface for an action in a workflow pipeline
    /// </summary>
    /// <typeparam name="T">WorkflowDataContext</typeparam>
    public interface IWorkflowAction<T> where T: WorkflowDataContext
    {
        void ExecuteAction(T context, IWorkflowProcessorContext processorContext);
    }
}
