using System;
using Simetron.GUI.Core;
using Simetron.GUI.Workbench;
using Simetron.Data;

namespace Simetron.GUI.Commands {
	public class QuitCommand : ICommand {
		public QuitCommand () {
		}

		public bool Run () {
			Workspace ws = WorkbenchSingleton.Instance.Workspace;
			IStore store = StoreFactory.Instance.CreateStore (typeof (Workspace),
									  StoreMode.XML);
			store.OpenConnection (GSimetronMain.MetadataFile);
			store.Write (ws);
			store.CloseConnection ();

			WorkbenchView view = WorkbenchSingleton.Instance.ActiveWorkbenchView;
			WorkbenchSingleton.Instance.RemoveWorkbenchView (view);
			view.Window.Destroy ();
			return true;
		}
	}
}
