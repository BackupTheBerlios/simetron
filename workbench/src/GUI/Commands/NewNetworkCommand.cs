namespace Simetron.GUI.Commands {
	using System;
	using System.Collections;
	using Simetron.Data;
	using Simetron.Data.NetworkTopology;
	using Simetron.GUI.Workbench;
	using Simetron.GUI.Dialogs;
	using Simetron.Logging;

	public class NewNetworkCommand : ICommand {
		public NewNetworkCommand () {
		}

		public bool Run () {
			Hashtable names = new Hashtable ();
			Reference[] projects = WorkbenchSingleton.Instance.Workspace.Projects.References;
			foreach (Reference r in projects) {
				Project p = (Project) WorkbenchSingleton.Instance.Workspace.Projects[r];
				string[] networkNames = p.Networks.Names;
				names[r.Name] = networkNames;
			}

			NewNetworkDialog dialog = new NewNetworkDialog (names);
			dialog.Run ();
			if (!dialog.OkClicked) {
				return false;
			}

			Reference projectReference = null;
			
			foreach (Reference r in projects) {
				string projectName = r.Name;
				if (projectName.Equals (dialog.ProjectName)) {
					projectReference = r;
				}
			}

			Logger.Assert (projectReference != null, "Project reference not found");				    
			Project p2 = (Project) WorkbenchSingleton.Instance.Workspace.Projects[projectReference];

			Reference networkReference = new Reference (dialog.NetworkName, projectReference.Mode);
			Network n = new Network ();
			p2.Networks[networkReference] = n;

			Logger.Debug ("Created new network : " + dialog.NetworkName + 
				      " , for project: " + dialog.ProjectName);

			return true;
		}
	}
}
