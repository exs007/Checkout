using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Test.DAL;
using GenFu;
using Xunit;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Workflow.Base;
using CheckoutKataAPI.Test.Workflow;
using Moq;

namespace CheckoutKataAPI.Test.Workflow
{
    public class WorkflowProcessorTest
    {
        private class Action1 : IWorkflowAction<FakeWorkflowDataContent>
        {
            public void ExecuteAction(FakeWorkflowDataContent context, IWorkflowProcessorContext processorContext)
            {
                context.StringData += "1";
            }
        }

        private class Action2 : IWorkflowAction<FakeWorkflowDataContent>
        {
            public void ExecuteAction(FakeWorkflowDataContent context, IWorkflowProcessorContext processorContext)
            {
                context.StringData += "2";
            }
        }

        private class Action3 : IWorkflowAction<FakeWorkflowDataContent>
        {
            public void ExecuteAction(FakeWorkflowDataContent context, IWorkflowProcessorContext processorContext)
            {
                context.StringData += "3";
            }
        }
        
        public WorkflowProcessorTest()
        {
        }

        [Fact]
        public void SetupMultipleActionsInWorkflowProcessorAndExecuteInOrder()
        {
            var provider = Mock.Of<IServiceProvider>();
            var actions = new List<ActionItem>()
            {
                new ActionItem(typeof(Action1)),
                new ActionItem(typeof(Action2)),
                new ActionItem(typeof(Action3)),
            };
            var workflowProcessor = new WorkflowProcessor<FakeWorkflowDataContent>(provider, actions);
            var context = new FakeWorkflowDataContent();
            context.StringData = "0";

            context =  workflowProcessor.Execute(context);
            Assert.Equal("0123", context.StringData);
        }

        [Fact]
        public void SetupZeroActionsInWorkflowProcessorAndExecute()
        {
            var provider = Mock.Of<IServiceProvider>();
            var actions = new List<ActionItem>()
            {
            };
            var workflowProcessor = new WorkflowProcessor<FakeWorkflowDataContent>(provider, actions);
            var context = new FakeWorkflowDataContent();
            context.StringData = "0";

            context =  workflowProcessor.Execute(context);
            Assert.Equal("0", context.StringData);
        }

        [Fact]
        public void SetupTheSameActionTwiceInWorkflowProcessorAndThrowException()
        {
            var provider = Mock.Of<IServiceProvider>();
            var actions = new List<ActionItem>()
            {
                new ActionItem(typeof(Action1)),
                new ActionItem(typeof(Action2)),
                new ActionItem(typeof(Action2)),
            };
            
            var exception = Assert.Throws<ArgumentException>(() => new WorkflowProcessor<FakeWorkflowDataContent>(provider, actions));
            Assert.Contains("The same action is specified more than one time", exception.Message);
        }
    }
}
