using System;
using System.Diagnostics;
using Gtk;
using GtkSharp;

namespace Simetron.GUI.Logging {
	public class GUIStatckFrameTraceListener : TraceListener {
		TextBuffer buffer;

		public GUIStatckFrameTraceListener () {
			Window window1 = new Window ("Log Output");
			ScrolledWindow window2 = new ScrolledWindow ();
			VBox box = new VBox (true, 0);				
			TextView view = new TextView ();
			view.Editable = false;
			buffer = view.Buffer;
			box.PackStart (view, true, true, 0);
			window2.AddWithViewport (box);
			window1.Add(window2);
			window1.SetDefaultSize (600,120);
			// handle the delete event to prevent the log window to be closed
			window1.DeleteEvent += new DeleteEventHandler (OnWindowDelete);
			window1.Iconify ();
			window1.ShowAll ();
		}

		private void OnWindowDelete (object o, DeleteEventArgs args) {
			// returning true avoids the window to be closed
			args.RetVal = true;
		}

		public override void Write (string message) {
			StackFrame sf = new StackFrame (5, true);
			string fileName = sf.GetFileName();
			int lineNumber = sf.GetFileLineNumber();
			string now = DateTime.Now.ToLongTimeString();
			string prefix = "[" + now + " - " + fileName + ":" + lineNumber + "] ";
			buffer.Text += prefix;
			buffer.Text += message;
		}
			
		public override void WriteLine (string message) {
			Write (message + '\n');
		}
	}
}
