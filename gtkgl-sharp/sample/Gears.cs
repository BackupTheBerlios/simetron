/*
 * GtkGL#  Graphics Library - http://www.simetron.org/gtkgl-sharp
 * 
 * Copyright (c) 2003, Olympum Group, All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer. Redistributions in binary
 * form must reproduce the above copyright notice, this list of conditions and
 * the following disclaimer in the documentation and/or other materials
 * provided with the distribution. Neither the name of the Olympum Group nor
 * the names of its contributors may be used to endorse or promote products
 * derived from this software without specific prior written permission. 
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 */

namespace GtkGL.Samples {
	using System;
	using GLib;
	using Gtk;
	using GtkSharp;
	using GtkGL;
	using GdkGL;
	using OpenGL;

	public class Gears {
		private static GtkGL.Area glarea;
		private static double view_rotx = 20.0;
		private static double view_roty = 30.0;
		private static double view_rotz = 0.0;
		private static uint gear1;
		private static uint gear2;
		private static uint gear3;
		private static double angle = 0.0;
		private static bool stop;

		public static void Main () {
			Application.Init ();
			GtkSharp.GtkGL.ObjectManager.Initialize ();

			Gtk.Window window = new Gtk.Window ("Gears");
			window.ReallocateRedraws = true;
			window.DeleteEvent += new DeleteEventHandler (OnDeleteEvent);

			int[] attrlist = {4,
					  8, 1,
					  9, 1,
					  10, 1,
					  5, 
					  0};

			glarea = new GtkGL.Area (attrlist);
			glarea.RequestSize = new System.Drawing.Size (200,200);

			glarea.Realized += new EventHandler (OnRealized);
			glarea.ConfigureEvent += new ConfigureEventHandler (OnConfigure);
			glarea.ExposeEvent += new ExposeEventHandler (OnExpose);
			glarea.MapEvent += new MapEventHandler (OnMap);
			glarea.UnmapEvent += new UnmapEventHandler (OnUnmap);
			window.Add (glarea);			
			window.Show ();
			glarea.Show ();
			Application.Run ();
		}

		private static void OnDeleteEvent (object obj, DeleteEventArgs args) {
			Application.Quit ();
		}

		private static void OnMap (object obj, MapEventArgs args) {
			GLib.Idle.Add (new IdleHandler (Animate));
			stop = false;
		}

		private static void OnUnmap (object obj, UnmapEventArgs args) {
			stop = true;
		}

		private static bool Animate () {
			angle += 2.0;
			glarea.QueueDraw ();
			return !stop;
		}

		private static void OnExpose (object obj, ExposeEventArgs args) {
			// only draw last expose
			//if (args.Event.count > 0) {
			//	return;
			//}
			
			// opengl functions can only be called if make current
			// returns true
			if (glarea.MakeCurrent() > 0) {
				GL.glClear (GL.GL_COLOR_BUFFER_BIT | 
					    GL.GL_DEPTH_BUFFER_BIT);

				GL.glPushMatrix ();
				GL.glRotated (view_rotx, 1.0, 0.0, 0.0);
				GL.glRotated (view_roty, 0.0, 1.0, 0.0);
				GL.glRotated (view_rotz, 0.0, 0.0, 1.0);

				GL.glPushMatrix ();
				GL.glTranslated (-3.0, -2.0, 0.0);
				GL.glRotated (angle, 0.0, 0.0, 1.0);
				GL.glCallList (gear1);
				GL.glPopMatrix ();

				GL.glPushMatrix ();
				GL.glTranslated (3.1, -2.0, 0.0);
				GL.glRotated (-2.0 * angle - 9.0, 0.0, 0.0, 1.0);
				GL.glCallList (gear2);
				GL.glPopMatrix ();

				GL.glPushMatrix ();
				GL.glTranslated (-3.1, 4.2, 0.0);
				GL.glRotated (-2.0 * angle - 25.0, 0.0, 0.0, 1.0);
				GL.glCallList (gear3);
				GL.glPopMatrix ();

				GL.glPopMatrix ();

				GL.glEnd();
				
				// Swap backbuffer to front
				glarea.SwapBuffers();		     
			}
		}

		private static void OnConfigure (object obj, ConfigureEventArgs args) {
			if (glarea.MakeCurrent() > 0) {
				GL.glViewport(0,
					      0, 
					      glarea.Allocation.width, 
					      glarea.Allocation.height);

				GL.glMatrixMode (GL.GL_PROJECTION);
				GL.glLoadIdentity ();
				float h = (float) glarea.Allocation.height / 
					(float) (glarea.Allocation.width);
				GL.glFrustum (-1.0f, 1.0f, -h, h, 5.0f, 60.0f);
				GL.glMatrixMode (GL.GL_MODELVIEW);
				GL.glLoadIdentity ();
				GL.glTranslatef (0.0f, 0.0f, -40.0f);
				GL.glEnd ();
			}
		}

		private static void OnRealized (object obj, EventArgs args) {
			if (glarea.MakeCurrent() > 0) {
				float[] pos = {5.0f, 5.0f, 10.0f, 0.0f};
				float[] red = {0.8f, 0.1f, 0.0f, 1.0f};
				float[] green = {0.0f, 0.8f, 0.2f, 1.0f};
				float[] blue = {0.2f, 0.2f, 1.0f, 1.0f};

				GL.glLightfv (GL.GL_LIGHT0, 
					      GL.GL_POSITION, 
					      pos);
				GL.glEnable (GL.GL_CULL_FACE);
				GL.glEnable (GL.GL_LIGHTING);
				GL.glEnable (GL.GL_LIGHT0);
				GL.glEnable (GL.GL_DEPTH_TEST);

				/* make the gears */
				gear1 = GL.glGenLists (1);
				GL.glNewList (gear1, GL.GL_COMPILE);
				GL.glMaterialfv (GL.GL_FRONT, 
						 GL.GL_AMBIENT_AND_DIFFUSE, 
						 red);
				gear (1.0, 4.0, 1.0, 20, 0.7);
				GL.glEndList ();

				gear2 = GL.glGenLists (1);
				GL.glNewList (gear2, GL.GL_COMPILE);
				GL.glMaterialfv (GL.GL_FRONT, 
					      GL.GL_AMBIENT_AND_DIFFUSE, 
					      green);
				gear (0.5, 2.0, 2.0, 10, 0.7);
				GL.glEndList ();

				gear3 = GL.glGenLists (1);
				GL.glNewList (gear3, GL.GL_COMPILE);
				GL.glMaterialfv (GL.GL_FRONT, 
						 GL.GL_AMBIENT_AND_DIFFUSE, 
						 blue);
				gear (1.3, 2.0, 0.5, 10, 0.7);
				GL.glEndList ();

				GL.glEnable (GL.GL_NORMALIZE);

				GL.glEnd ();
			}
		}

		private static void gear(double inner_radius,
					 double outer_radius,
					 double width,
					 int   teeth,
					 double tooth_depth) {
			int i;
			double r0, r1, r2;
			double angle, da;
			double u, v, len;

			r0 = inner_radius;
			r1 = outer_radius - tooth_depth / 2.0;
			r2 = outer_radius + tooth_depth / 2.0;
			
			da = 2.0 * 3.14159 / teeth / 4.0;

			GL.glShadeModel(GL.GL_FLAT);

			GL.glNormal3d(0.0, 0.0, 1.0);

			/* draw front face */
			GL.glBegin(GL.GL_QUAD_STRIP);
			for (i = 0; i <= teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glVertex3d(r0 * Math.Cos(angle), 
					      r0 * Math.Sin(angle), 
					      width * 0.5);
				GL.glVertex3d(r1 * Math.Cos(angle), 
					      r1 * Math.Sin(angle), 
					      width * 0.5);
				if (i < teeth) {
					GL.glVertex3d(r0 * Math.Cos(angle), 
						      r0 * Math.Sin(angle), 
						      width * 0.5);
					GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
						      r1 * Math.Sin(angle + 3 * da), 
						      width * 0.5);
				}
			}
			GL.glEnd();

			/* draw front sides of teeth */
			GL.glBegin(GL.GL_QUADS);
			da = 2.0 * 3.14159 / teeth / 4.0;
			for (i = 0; i < teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glVertex3d(r1 * Math.Cos(angle), 
					   r1 * Math.Sin(angle), 
					   width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + da), 
					      r2 * Math.Sin(angle + da), 
					      width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					      r2 * Math.Sin(angle + 2 * da), 
					      width * 0.5);
				GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					      r1 * Math.Sin(angle + 3 * da), 
					      width * 0.5);
			}
			GL.glEnd();
			
			GL.glNormal3d(0.0, 0.0, -1.0);
			
			/* draw back face */
			GL.glBegin(GL.GL_QUAD_STRIP);
			for (i = 0; i <= teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glVertex3d(r1 * Math.Cos(angle), 
					   r1 * Math.Sin(angle), 
					   -width * 0.5);
				GL.glVertex3d(r0 * Math.Cos(angle), 
					   r0 * Math.Sin(angle), 
					   -width * 0.5);
				if (i < teeth) {
					GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
						   r1 * Math.Sin(angle + 3 * da), 
						   -width * 0.5);
					GL.glVertex3d(r0 * Math.Cos(angle), 
						   r0 * Math.Sin(angle), 
						   -width * 0.5);
				}
			}
			GL.glEnd();
			
			/* draw back sides of teeth */
			GL.glBegin(GL.GL_QUADS);
			da = 2.0 * 3.14159 / teeth / 4.0;
			for (i = 0; i < teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					   r1 * Math.Sin(angle + 3 * da), 
					   -width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					      r2 * Math.Sin(angle + 2 * da), 
					      -width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + da), 
					      r2 * Math.Sin(angle + da), 
					      -width * 0.5);
				GL.glVertex3d(r1 * Math.Cos(angle), 
					      r1 * Math.Sin(angle), -width * 0.5);
			}
			GL.glEnd();
			
			/* draw outward faces of teeth */
			GL.glBegin(GL.GL_QUAD_STRIP);
			for (i = 0; i < teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glVertex3d(r1 * Math.Cos(angle), 
					      r1 * Math.Sin(angle), 
					      width * 0.5);
				GL.glVertex3d(r1 * Math.Cos(angle), 
					      r1 * Math.Sin(angle), 
					      -width * 0.5);
				u = r2 * Math.Cos(angle + da) - r1 * Math.Cos(angle);
				v = r2 * Math.Sin(angle + da) - r1 * Math.Sin(angle);
				len = Math.Sqrt(u * u + v * v);
				u /= len;
				v /= len;
				GL.glNormal3d(v, -u, 0.0);
				GL.glVertex3d(r2 * Math.Cos(angle + da), 
					      r2 * Math.Sin(angle + da), 
					      width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + da), 
					      r2 * Math.Sin(angle + da), 
					      -width * 0.5);
				GL.glNormal3d(Math.Cos(angle), 
					      Math.Sin(angle), 
					      0.0);
				GL.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					      r2 * Math.Sin(angle + 2 * da), 
					      width * 0.5);
				GL.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					      r2 * Math.Sin(angle + 2 * da), 
					      -width * 0.5);
				u = r1 * Math.Cos(angle + 3 * da) 
					- r2 * Math.Cos(angle + 2 * da);
				v = r1 * Math.Sin(angle + 3 * da) 
					- r2 * Math.Sin(angle + 2 * da);
				GL.glNormal3d(v, -u, 0.0);
				GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					      r1 * Math.Sin(angle + 3 * da), 
					      width * 0.5);
				GL.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					      r1 * Math.Sin(angle + 3 * da), 
					      -width * 0.5);
				GL.glNormal3d(Math.Cos(angle), 
					      Math.Sin(angle), 
					      0.0);
			}
			
			GL.glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), width * 0.5);
			GL.glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), -width * 0.5);
			
			GL.glEnd();
			
			GL.glShadeModel(GL.GL_SMOOTH);
			
			/* draw inside radius cylinder */
			GL.glBegin(GL.GL_QUAD_STRIP);
			for (i = 0; i <= teeth; i++) {
				angle = i * 2.0 * 3.14159 / teeth;
				GL.glNormal3d(-Math.Cos(angle), -Math.Sin(angle), 0.0);
				GL.glVertex3d(r0 * Math.Cos(angle), 
					   r0 * Math.Sin(angle), 
					   -width * 0.5);
				GL.glVertex3d(r0 * Math.Cos(angle), 
					      r0 * Math.Sin(angle), 
					      width * 0.5);
			}
			GL.glEnd();
		}
	}
}
