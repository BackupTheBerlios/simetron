using Gtk;
using GtkSharp;	

public class ParentTest {
	public static void Main () {
		Application.Init ();
		Window w = new Window ("Hi");
		w.DeleteEvent += new DeleteEventHandler (OnDeleteEvent);
		w.Show ();
		Widget widget = w.Parent;
		Application.Run ();
	}

	private static void OnDeleteEvent (object o, DeleteEventArgs args) {
		Application.Quit ();
	}
}
		
