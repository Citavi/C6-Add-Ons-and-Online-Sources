﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public static class ReflectionHelper
    {
        public static TResult Field<TObject, TResult>(this TObject tObject, string fieldName) where TObject : class where TResult : class
        {
            return
                tObject
                    .Members(fieldName, MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .OfType<FieldInfo>()
                    .FirstOrDefault(field => field.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))?
                    .GetValue(tObject)
                    .ConvertTo<TResult>();
        }

        public static object Invoke<TObject>(this TObject tObject, string methodName, params object[] parameters) where TObject : class
        {
            return
                tObject
                   .Members(methodName, MemberTypes.Method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                   .OfType<MethodInfo>()
                   .FirstOrDefault(method => method.GetParameters().AreEqual(parameters))
                   ?.Invoke(tObject, parameters);
        }

        private static IEnumerable<MemberInfo> Members<TObject>(this TObject tObject, string memberName, MemberTypes memberType, BindingFlags bindingFlags)
        {
            return
                tObject
                    .GetType()
                    .GetMembers(bindingFlags)
                    .Where(prop => prop.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && prop.MemberType == memberType)
                    .ToList();
        }

        private static bool AreEqual(this ParameterInfo[] parameterInfos, object[] parameters)
        {
            if (parameterInfos?.Length == parameters?.Length)
            {
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    if (parameterInfos[i].ParameterType != parameters[i].GetType())
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;
        }

        private static TResult ConvertTo<TResult>(this object obj)
        {
            if (obj is null)
            {
                return default;
            }

            return (TResult)Convert.ChangeType(obj, typeof(TResult));
        }
    }
}
