using Pong.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pong.Helpers;

public class AssemblyHelper
{
    public static List<GameObject> ScanGameObjects()
    {
        List<GameObject> gameObjectsFound = new();

        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        Type gameObjectType = typeof(GameObject);

        foreach (Type type in currentAssembly.GetTypes())
        {
            if (gameObjectType.IsAssignableFrom(type) && !type.IsInterface)
            {
                if (Activator.CreateInstance(type) is GameObject instance)
                {
                    gameObjectsFound.Add(instance);
                }
            }
        }

        return gameObjectsFound;
    }

    public static bool ChildClassOverrides(Type baseType, Type childType, string methodName)
    {
        MethodInfo methodInfo = childType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);

        string declaringTypeFullName = methodInfo.DeclaringType.FullName;

        return !declaringTypeFullName.Equals(baseType.FullName, StringComparison.OrdinalIgnoreCase);
    }
}
