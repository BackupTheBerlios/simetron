namespace Simetron.GUI.Commands {
	using System;
	using Simetron.GUI.Dialogs;

	public class AboutCommand : ICommand {
		public AboutCommand () {
		}

		public bool Run () {
			AboutDialog dialog = new AboutDialog ();
			dialog.Run ();
			return true;
		}
	}
}
