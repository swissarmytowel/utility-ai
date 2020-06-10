using System;

namespace UtilityAI_Base.CustomAttributes
{
    public class ActionSelectorAttribute : Attribute
    {
        public string Selector;

        public ActionSelectorAttribute(string name) {
            Selector = name;
        }
    }
}