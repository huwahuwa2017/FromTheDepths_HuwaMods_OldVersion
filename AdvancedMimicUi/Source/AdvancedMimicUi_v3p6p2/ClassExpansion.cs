using System.Collections.Generic;

namespace HuwaTech
{
    public abstract class AddMemberBase<CurrentType>
    {
        public CurrentType __instance;
    }

    public static class ClassExpansion<CurrentType, AddMemberType> where AddMemberType : AddMemberBase<CurrentType>, new()
    {
        private static readonly Dictionary<CurrentType, AddMemberType> AddMemberDictionary = new Dictionary<CurrentType, AddMemberType>();

        public static AddMemberType Access(CurrentType CurrentInstance)
        {
            if (!AddMemberDictionary.ContainsKey(CurrentInstance))
            {
                AddMemberDictionary.Add(CurrentInstance, new AddMemberType
                {
                    __instance = CurrentInstance
                }
                );
            }

            return AddMemberDictionary[CurrentInstance];
        }
    }
}
