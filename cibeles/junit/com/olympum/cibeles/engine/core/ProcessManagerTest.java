package com.olympum.cibeles.engine.core;

import junit.framework.TestCase;
import java.util.HashMap;

public class ProcessManagerTest extends TestCase {

	public ProcessManagerTest(String arg0) {
		super(arg0);
	}

	public static void main(String[] args) {
		junit.textui.TestRunner.run(ProcessManagerTest.class);
	}

	public void testRun() {
		System.setProperty("WFDL", "com/olympum/cibeles/engine/core/WFDL-example-old.xml");
		System.setProperty("RESOURCES", "com/olympum/cibeles/engine/core/resources-old.xml");
		HashMap input = new HashMap();
		input.put("pv1", new Integer(10));
		System.out.println(input);		
		HashMap output = ProcessManager.run("SimpleProcess", input);
		System.out.println(output);		
	}
}
