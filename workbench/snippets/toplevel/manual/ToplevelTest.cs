using System;
using Gtk;
using GtkSharp;

public class ToplevelTest {
	static MenuItem item;
	static Label label;
	static Window window;

	public static void Main () {
		Application.Init ();
		window = new Window ("Top level test");
		VBox box = new VBox (true, 0);
		MenuBar mb = new MenuBar ();
                
                item = new MenuItem ("_Test");
		mb.Append (item); 
		box.PackStart (mb, true, true, 0);
		label = new Label ("Text");
		box.PackStart (label, true, true, 0);
		window.Add (box);
		window.ShowAll ();
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
