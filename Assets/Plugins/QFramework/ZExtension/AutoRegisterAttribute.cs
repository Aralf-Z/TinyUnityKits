using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoRegisterAttribute : Attribute
    {
        public Type Archi { get; }

        public AutoRegisterAttribute(Type archi)
        {
            Archi = archi;
        }
    }
}
