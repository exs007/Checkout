namespace CheckoutKataAPI.Workflow.Base
{
    /// <summary>
    /// Interface for processing context through a pipeline
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWorkflowProcessor<T> where T: WorkflowDataContext
    {
        //process a context throw steps in a processor
        T Execute(T context);
    }
}
