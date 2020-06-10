using System;

namespace UtilityAI_Base.CustomAttributes
{
    public class ConsiderationsQualifierAttribute : Attribute
    {
        public string Description;
        public Type ObjectType;
        public ConsiderationsQualifierAttribute(string description, Type objectType) {
            Description = description;
            ObjectType = objectType;
        }
    }
}