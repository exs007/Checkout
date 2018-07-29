using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GenFu;
using Xunit;
using CheckoutKataAPI.Workflow.Base;
using Moq;
using CheckoutKataAPI.Constants;

namespace CheckoutKataAPI.Test.Workflow
{
    public class SetupOrderedProductsInfoTest
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
        
        public SetupOrderedProductsInfoTest()
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
            Assert.Contains(MessageConstants.LIST_BASED_WORKFLOW_PROCESSOR_NOT_ALLOW_SAME_ACTION_MULTIPLE_TIME, exception.Message);
        }
    }
}
