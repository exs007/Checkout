namespace CheckoutKataAPI.Workflow.Base
{
    public interface IWorkflowProcessor<T> where T: WorkflowDataContext
    {
        //process a context throw steps in a processor
        T Execute(T context);
    }
}
