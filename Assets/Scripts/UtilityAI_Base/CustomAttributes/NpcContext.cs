using System;

namespace UtilityAI_Base.CustomAttributes
{
    public class NpcContext : Attribute
    {
        public string Name;

        public NpcContext(string name) {
            Name = name;
        }
    }

    public class NpcContextVar : Attribute
    {
        
    }
}