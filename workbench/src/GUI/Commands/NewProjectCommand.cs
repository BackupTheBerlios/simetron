namespace Simetron.GUI.Commands {
	using System;
	using Simetron.Data;
	using Simetron.GUI.Workbench;
	using Simetron.GUI.Dialogs;
	using Simetron.Logging;

	public class NewProjectCommand : ICommand {
		public NewProjectCommand () {
		}

		public bool Run () {
			/*
			string[] existingProjects = WorkbenchSingleton.Instance.Workspace.Projects.Names;
			NewProjectDialog dialog = new NewProjectDialog (existingProjects);
			dialog.Run ();
			if (!dialog.OkClicked) {
				return false;
			}
			Logger.Debug ("Creating new project : " + dialog.ProjectName);
			Reference reference = new Reference ();
			reference.Name = dialog.ProjectName;
			reference.Mode = dialog.Mode;
			Project p = new Project ();
			WorkbenchSingleton.Instance.Workspace.Projects[reference] = p;
			*/
			return true;
		}
	}
}
