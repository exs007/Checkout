using System;
using Microsoft.Extensions.DependencyInjection;

namespace CheckoutKataAPI.Workflow.Base
{
    public class WorkflowProcessorContext : IWorkflowProcessorContext
    {
        private readonly IServiceProvider _provider;

        public WorkflowProcessorContext(IServiceProvider provider)
        {
            _provider = provider;
        }

        public T Resolve<T>()
        {
            return _provider.GetService<T>();
        }

        public virtual void Dispose()
        {
        }
    }
}
