﻿using System;
using System.Linq;
using System.Reflection;

using Funky;

namespace Reflections
{
    internal static class ReflectedMethods
    {
        private static MethodInfo MakeClosuredGetCustomAttributesMethod(Type type)
        {
            return GetCustomAttributes.MakeGenericMethod(type);
        }

        private static readonly Func<Type, MethodInfo> ClosuredGetCustomAttributesMethod = MakeClosuredGetCustomAttributesMethod;

        private static readonly Func<Type, MethodInfo> MemoizedGetCustomAttributesMethod = ClosuredGetCustomAttributesMethod.Memoize(true);

        public static readonly MethodInfo GetCustomAttributes =
            typeof(CustomAttributeExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(mi => mi.Name == "GetCustomAttributes")
                .Where(mi => mi.GetGenericArguments().Count() == 1)
                .Where(mi => mi.GetParameters().Count() == 2)
                .Where(mi => mi.GetParameters()[0].ParameterType == typeof(MemberInfo))
                .Single(mi => mi.GetParameters()[1].ParameterType == typeof(bool));

        public static MethodInfo MakeClosuredGetCustomAttributesMethodForType(Type type)
        {
            return MemoizedGetCustomAttributesMethod(type);
        }
    }
}