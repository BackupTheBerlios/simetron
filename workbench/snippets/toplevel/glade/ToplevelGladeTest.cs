using System;
using Gtk;
using Glade;
using GtkSharp;

public class ToplevelGladeTest {
	static MenuItem item;
	static Label label;
	static Window window;

	public static void Main () {
		Application.Init ();
		XML gxml = new XML ("toplevel.glade",
				    "window1",
				    null);
		window = (Window) gxml["window1"];
		item = (MenuItem) gxml["new1"];
		label = (Label) gxml["label1"];
	        window.DeleteEvent += new DeleteEventHandler (OnDelete);
		Application.Run ();
	}

	public static void OnDelete (object obj, DeleteEventArgs args) {
		Console.WriteLine (window.Toplevel.Handle);
		Console.WriteLine (item.Toplevel.Handle);
		Console.WriteLine (label.Toplevel.Handle);
		Application.Quit ();
	}
}
