using System;

namespace CheckoutKataAPI.Workflow.Base
{
    /// <summary>
    /// Interface for an executing helper context, at least should provide abitity to get dependencies
    /// </summary>
    public interface IWorkflowProcessorContext : IDisposable
    {
        T Resolve<T>();
    }
}