namespace Simetron.GUI.Commands 
{
	using System;
	using Simetron.GUI.Core;
	using Simetron.GUI.Workbench;
	using Simetron.Data;
	
	public class CloseWindowCommand : ICommand 
	{
		public bool Run () 
		{
			WorkbenchView view = WorkbenchSingleton.Instance.CurrentView;
			WorkbenchSingleton.Instance.RemoveWorkbenchView (view);
			return true;
		}
	}
}
