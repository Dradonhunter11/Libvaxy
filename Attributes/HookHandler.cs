﻿using System;
using System.Reflection;

namespace Libvaxy.Attributes
{
	internal class HookHandler
	{
		internal static void ApplyHooks()
		{
			foreach (MethodInfo method in Reflection.GetMethodsWithAttribute<HookAttribute>())
			{ 
				foreach (HookAttribute hookAttribute in method.GetCustomAttributes(typeof(HookAttribute), false))
				{
					Type parentType = hookAttribute.eventParentType;
					string hookName = hookAttribute.hookName;
					EventInfo hookEvent = parentType.GetEvent(hookName);

					string fullHookName = $"{parentType.FullName}.{hookName}";
					if (hookEvent == null)
						throw new LibvaxyException($"Attempted to hook into an unknown method: {fullHookName}");

					hookEvent.AddEventHandler(null, Delegate.CreateDelegate(hookEvent.EventHandlerType, method));
					Libvaxy.Logger.Info($"Registered hook: {fullHookName}\nBy: {method.FullMemberName()}");
				}
			}
		}
	}
}
