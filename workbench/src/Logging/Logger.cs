// always compile with /define:TRACE (otherwise no trace)
// compile with /define:DEBUG for debug
// compile with /define:CONSOLE for Console.Out
// compile with /debug to make symbols available for stack frame
// run with --debug to print class and line numbers
namespace Simetron.Logging 
{
	using System;
	using System.Configuration;
	using System.Diagnostics;
	using System.IO;

	public sealed class Logger 
	{
		private Logger () 
		{
		}

		static Logger () 
		{
			Init ();
		}

		private static void Init () 
		{
			System.Diagnostics.Debug.Listeners.Add (new StackFrameTraceListener (Console.Out));
			string logfile = ConfigurationSettings.AppSettings.Get ("LogFile"); 
			if (logfile != null) {
				FileStream stream = new FileStream (logfile,
								    FileMode.OpenOrCreate,
								    FileAccess.Write);
				StreamWriter writer = new StreamWriter (stream);
				writer.BaseStream.Seek (0, SeekOrigin.End);
				System.Diagnostics.Debug.Listeners.Add (new StackFrameTraceListener (writer));
			}
			Logger.Debug ("Initialized logging");
		}

		[Conditional("DEBUG")]
		public static void Debug (string message) 
		{
			System.Diagnostics.Debug.WriteLine (message);
		}

		public static void Fail (string message) 
		{
			System.Diagnostics.Trace.Fail (message);
		}

		// note that assertions only work on DEBUG mode! this is in purpose
		[Conditional("DEBUG")]
		public static void Assert (bool expression, string shortMessage) 
		{
			System.Diagnostics.Debug.Assert (expression, shortMessage);
		}

		[Conditional("DEBUG")]
		public static void Assert (bool expression) 
		{
			System.Diagnostics.Debug.Assert (expression);
		}
	}
}
