using System;

namespace CheckoutKataAPI.Workflow.Base
{
    //interface for an executing helper context, at least should provide abitity to get dependencies
    public interface IWorkflowProcessorContext : IDisposable
    {
        T Resolve<T>();
    }
}