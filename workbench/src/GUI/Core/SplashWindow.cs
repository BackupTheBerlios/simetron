namespace Simetron.GUI.Core {
	using System;
	using System.Threading;
	using Gtk;
	using Gdk;

	public sealed class SplashWindow {
		private static Gtk.Dialog window;
		private static Gtk.Button label;
		private static bool readyToBeDestroyed = false;
		private readonly static object destroyLock = new object ();
		private static ThreadNotify notify = new ThreadNotify (new ReadyEvent (DestroyPrivate));

		private SplashWindow () {}

		static SplashWindow () {
			bool splashEnabled = true;
			// TODO : configuration to disable splash
			if (splashEnabled) {
				window = new Gtk.Dialog ();
				window.Title = "Starting Simetron ...";
				window.Decorated = false;
				window.Modal = true;
				window.SetPosition (WindowPosition.Center);
				window.HasSeparator = true;

				Gdk.Pixbuf pixbuf = new Pixbuf (null, 
						     "pixmaps/splash.jpg");
				Gtk.Image splashImage = new Gtk.Image (pixbuf);
				window.VBox.PackStart (splashImage, false, false, 0);

				label = new Gtk.Button ();
				label.Relief = ReliefStyle.None;
				//window.ActionArea.PackEnd (label, false, false, 0);
				window.AddActionWidget (label, 0);
				//window.SetResponseSensitive (0, false);
				label.Clicked += new EventHandler (OnButtonClicked);
				window.ShowAll ();
				
				Thread t = new Thread (new ThreadStart (TimedDestroy));
				t.IsBackground = true;
				t.Start ();
			}
		}

		private static void OnButtonClicked (object o, EventArgs args) {
			DestroyPrivate ();
		}

		public static void Update (string message) {
			if (window != null) {
				label.Label = message;
				while (Application.EventsPending ()) {
					Application.RunIteration ();
				}
			}
		}

		public static void Destroy () {
			if (window != null) {
				Monitor.Enter (destroyLock);
				readyToBeDestroyed = true;
				Monitor.Pulse (destroyLock);
				Monitor.Exit (destroyLock);
			}
		}

		private static void TimedDestroy () {
			Thread.Sleep (2000); // show the splash at least for 2 sec
			Monitor.Enter (destroyLock);
			while (!readyToBeDestroyed) {
				Monitor.Wait (destroyLock);					
			}
			notify.WakeupMain ();
			Monitor.Exit (destroyLock);	
		}

		private static void DestroyPrivate () {
			window.Hide ();
		}
	}
}
