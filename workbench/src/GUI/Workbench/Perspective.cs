using System;
using Gtk;
using GtkSharp;
using Gdk;
using Simetron.Data;
using Simetron.GUI.Editors;
using Simetron.Logging;

namespace Simetron.GUI.Workbench {
	public class Perspective : HPaned {
		private NonCloseableNotebook leftNotebook;
		private NonCloseableNotebook bottomNotebook;
		private CloseableNotebook editorNotebook;
		private Explorer explorer;

		public Perspective () : base () {
			// create a notebook on the left pane
			leftNotebook = new NonCloseableNotebook ();
			this.Pack1 (leftNotebook, true, false);

			// create a vertical splitter on the right pane
			VPaned vsplitter = new VPaned ();
			this.Pack2 (vsplitter, true, false);

			// create a notebook on the top pane
			editorNotebook = new CloseableNotebook ();
			vsplitter.Pack1 (editorNotebook, true, false);
			
			// register to be notified of when a page is being removed
			editorNotebook.OnRemovingPage += 
				new RemovingPageHandler (OnEditorClosing);

			// create another notebook on the bottom pane
			bottomNotebook = new NonCloseableNotebook ();
			vsplitter.Pack2 (bottomNotebook, true, false);

			// create the explorer and add it to the left notebook
			explorer = new Explorer ();
			leftNotebook.AppendPage (explorer, "Explorer");
			explorer.ShowAll ();
			
		}

		public bool LeftNotebookVisible {
			get {
				return leftNotebook.Visible;
			} 
			set {
				if (value) {
					leftNotebook.ShowAll ();
				} else {
					leftNotebook.HideAll ();
				}
			}
		}

		public bool BottomNotebookVisible {
			get {
				return bottomNotebook.Visible;
			} 
			set {
				if (value) {
					bottomNotebook.ShowAll ();
				} else {
					bottomNotebook.HideAll ();
				}
			}
		}
		
		public void OpenActiveDocument (object o) {
			//Project project = o as Project;
			//explorer.PopulateStore (project);
			//NetworkEditor editor = new NetworkEditor (project.Network);
			//editorNotebook.AppendPage (editor, project.ProjectIdentification);
			//editorNotebook.ShowAll ();
		}

		private void OnEditorClosing (object o, RemovingPageEventArgs args) {
			Logger.Debug ("Requested to close an object of type " + o);
			//args.Removable = true;
		}
	}
}
