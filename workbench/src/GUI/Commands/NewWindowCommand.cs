namespace Simetron.GUI.Commands {
	using System;
	using Simetron.GUI.Workbench;
	
	public class NewWindowCommand : ICommand {
		public NewWindowCommand () {
		}

		public bool Run () {
			WorkbenchSingleton.Instance.CreateWorkbenchView ();
			return true;
		}
	}
}
