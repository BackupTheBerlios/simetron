using System;
using System.Diagnostics;
using System.IO;

// always compile with /define:TRACE (otherwise no trace)
// compile with /define:DEBUG for debug
// compile with /define:CONSOLE for Console.Out
// compile with /define:GUI for Gtk Window for log
// compile with /debug to make symbols available for stack frame
// run with --debug to print class and line numbers
namespace Simetron.Logging {
	public sealed class Logger {
		private Logger () {}

		static Logger () {
			Init ();
		}

		private static void Init () {
			// TODO: the init should work based on config
			System.Diagnostics.Debug.Listeners.Add (new StackFrameTraceListener (Console.Out));
			FileStream stream = new FileStream ("simetron.log",
						 FileMode.OpenOrCreate,
						 FileAccess.Write);
			StreamWriter writer = new StreamWriter (stream);
			writer.BaseStream.Seek (0, SeekOrigin.End);
			System.Diagnostics.Debug.Listeners.Add (new StackFrameTraceListener (writer));
			Logger.Debug ("Initialized console logging");
		}

		[Conditional("DEBUG")]
		public static void Debug (string message) {
			System.Diagnostics.Debug.WriteLine (message);
		}

		public static void Fail (string message) {
			System.Diagnostics.Trace.Fail (message);
		}

		// note that assertions only work on DEBUG mode! this is in purpose
		[Conditional("DEBUG")]
		public static void Assert (bool expression, string shortMessage) {
			System.Diagnostics.Debug.Assert (expression, shortMessage);
		}

		[Conditional("DEBUG")]
		public static void Assert (bool expression) {
			System.Diagnostics.Debug.Assert (expression);
		}

	}
}
