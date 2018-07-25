using System;

namespace CheckoutKataAPI.Workflow.Base
{
    //stores a description of an action for a processor
    public class ActionItem
    {   
        public Type ActionType { get; }

        public ActionItem(Type actionType)
        {
            ActionType = actionType;
        }
    }
}
