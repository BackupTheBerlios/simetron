using System;
using System.Diagnostics;
using System.IO;

/*
  Demonstrates the use of TraceListener to print additional information
  To compile in mono:
  $ mcs -define:DEBUG -define:TRACE  --debug Diagnostics.cs
  To run in mono:
  $ mono --debug Diagnostics.exe
  
  If the debug/trace defines and flags are not set, this code is not even
  compiled as part of the assembly, which is a great benefit over Java's
  logging and Log4j.
 */
namespace Snippets {
	public class TraceTest {
		public static void Main () {
			Debug.Listeners.Add(new MyTraceListener (Console.Out));
			Debug.AutoFlush = true;
			Debug.WriteLine("Entering Main");
			Debug.WriteLine("Exiting Main");
			Trace.WriteLine("Tracing ...");
			Trace.Assert (1 == 2, "1 == 2");
			Debug.Unindent();
		}
	}

	public class MyTraceListener : TextWriterTraceListener {
		public MyTraceListener () : base () {}

		public MyTraceListener (Stream stream) : base (stream) {}

		public MyTraceListener (string file) : base (file) {}

		public MyTraceListener (TextWriter writer) : base (writer) {}

		public MyTraceListener (Stream stream, string name) : base (stream, name) {}

		public MyTraceListener (string file, string name) : base (file, name) {}

		public MyTraceListener (TextWriter writer, string name) : base (writer, name) {}

		public override void Write (string message) {
			StackFrame sf = new StackFrame (5, true);
			string fileName = sf.GetFileName();
			int lineNumber = sf.GetFileLineNumber();
			string methodName = sf.GetMethod ().Name;
			string prefix = "["+ fileName + ":" + lineNumber +" (" + methodName + ")]";
			base.Write ( prefix + message);
		}
	}
}
