namespace Simetron.GUI.Commands 
{
	using System;
	using System.Collections;
	using System.Reflection;
	using Simetron.Logging;

	public sealed class CommandFactory 
	{
		private static Hashtable commands;
		private static string ns;

		static CommandFactory () 
		{
			commands = new Hashtable ();
			ns = (typeof (CommandFactory)).Namespace;
		}

		private CommandFactory () 
		{
		}

		public static ICommand CreateCommand (string commandName) 
		{
			if (commands.ContainsKey (commandName)) {
				return (ICommand) commands[commandName];
			} else {
				string typeName = ns + "." + commandName;
				Type type = Type.GetType (typeName);
				Logger.Assert (type != null, commandName + " not found");
				ConstructorInfo consInfo = type.GetConstructor (Type.EmptyTypes);
				object obj = consInfo.Invoke (null);
				ICommand command = obj as ICommand;
				Logger.Assert (command != null, commandName + " is not a command");
				commands[commandName] = command;
				return command;
			}
		}
	}
}
