//
//  SplashWindow.cs  - Splash window during startup
//
//  Author:
//    Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
//  Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
//  All Rights Reserved
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

namespace Simetron.GUI.Core 
{
	using System;
	using System.Threading;
	using Gtk;
	using Gdk;
	using Glade;
	using Simetron.GUI.Workbench;

	sealed class SplashWindow 
	{
		static Gtk.Window window;
		static Gtk.Button label;
		static bool readyToBeDestroyed;
		static object destroyLock;
		static ThreadNotify notify;

		static SplashWindow () 
		{
			// register a callback handler (DestroyPrivate) from main thread
			notify = new ThreadNotify (new ReadyEvent (DestroyPrivate));

			// dummy object used as a lock between the splash thread and 
			// the main thread
			destroyLock = new object ();
			readyToBeDestroyed = false;

			// create static window with libglade
			XML gxml = new Glade.XML (null,
						  WorkbenchSingleton.GLADEFILE,
						  "SplashWindow",
						  null);

			// get a handle on the widgets
			window = (Gtk.Window) gxml["SplashWindow"];
			label = (Gtk.Button) gxml["label"];
			Gtk.Image image = (Gtk.Image) gxml["image"];
			
			// set the image and remove the borders
			Gdk.Pixbuf pixbuf = new Pixbuf (null, "splash.jpg");
			image.Pixbuf = pixbuf;
			Gtk.Image splashImage = new Gtk.Image (pixbuf);	
			window.Decorated = false;

			// set hanlder for button
			label.Clicked += new EventHandler (OnButtonClicked);
			
			// show all
			window.ShowAll ();
			
			// start thread to show the splash for at least 1/2 sec
			Thread t = new Thread (new ThreadStart (TimedDestroy));
			t.IsBackground = true;
			t.Start ();
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

		static void OnButtonClicked (object o, EventArgs args) 
		{
			window.Hide ();
		}

		static void TimedDestroy () 
		{
			Thread.Sleep (500); // show the splash at least for 0.5 sec
			Monitor.Enter (destroyLock);
			while (!readyToBeDestroyed) {
				Monitor.Wait (destroyLock);					
			}
			// make the main thread call DestroyPrivate 
			// whenever it becomes idle from processing pending events
			notify.WakeupMain ();
			Monitor.Exit (destroyLock);	
		}

		static void DestroyPrivate () 
		{
			if (window != null) { 
				window.Destroy ();
			}
		}

		// private constructor to avoid initialization
		SplashWindow () {}
	}
}
