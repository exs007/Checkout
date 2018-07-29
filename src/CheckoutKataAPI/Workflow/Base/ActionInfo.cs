using System;

namespace CheckoutKataAPI.Workflow.Base
{
   /// <summary>
   /// Stores a description of an action for a processor
   /// </summary>
    public class ActionItem
    {   
        public Type ActionType { get; }

        public ActionItem(Type actionType)
        {
            ActionType = actionType;
        }
    }
}
