package com.olympum.cibeles.engine.core;

import junit.framework.TestCase;

public class ExecutionObjectTest extends TestCase {

	private static class ExecObjImpl extends ExecutionObject {
		ExecObjImpl(String name, String[] portNames, Object dumym) {
			super(name,portNames);
		}
		
		public void startExecution() {}
	}
	
	public ExecutionObjectTest(String arg0) {
		super(arg0);
	}

	public static void main(String[] args) {
		junit.textui.TestRunner.run(ExecutionObjectTest.class);
	}

	public void testGetName() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj = new ExecObjImpl(name,portNames,null);
		assertEquals(obj.getName(),name);
	}

	public void testGetPortValue() {
		String[] portNames = {"a","b"};
		String name = "A1";
		Object value = new Object();
		ExecObjImpl obj = new ExecObjImpl(name,portNames,null);
		obj.setPortValue("a", value);
		assertEquals(obj.getPortValue("a"),value);
	}

	public void testSetPortValue() {
		String[] portNames = {"a","b"};
		String name = "A1";
		Object value = new Object();
		ExecObjImpl obj = new ExecObjImpl(name,portNames,null);
		obj.setPortValue("a", value);
		assertEquals(value,DataStore.getValue(obj,0));		
	}
	
	public void testSetPortValue2() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);
		Object value1 = new Object();
		obj1.setPortValue("a", value1);
		ExecObjImpl obj2 = new ExecObjImpl(name,portNames,null);
		Object value2 = new Object();
		obj2.setPortValue("a", value2);
		assertEquals(value1,DataStore.getValue(obj1,0));
		assertEquals(value2,DataStore.getValue(obj2,0));				
	}
	
	public void testGetAllPortNames() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);
		assertEquals(portNames,obj1.getAllPortNames());
	}

	public void testIsNotRunning() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);
		assertTrue(obj1.isNotRunning());		
	}

	public void testReset() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);
		obj1.setPortValue("a", new Object());		
		obj1.setPortValue("b", new Object());
		obj1.reset();
		assertEquals(obj1.getPortValue("a"),null);
		assertEquals(obj1.getPortValue("b"),null);		
	}

	public void testContains() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);
		assertTrue(obj1.contains("a"));
		assertTrue(obj1.contains("b"));
		assertFalse(obj1.contains("c"));				
	}

	public void testGetState() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);		
		assertEquals(obj1.getState(),State.NOT_RUNNING);
	}

	public void testSetState() {
		String[] portNames = {"a","b"};
		String name = "A1";
		ExecObjImpl obj1 = new ExecObjImpl(name,portNames,null);		
		obj1.setState(State.RUNNING);
		assertEquals(obj1.getState(),State.RUNNING);		
	}
}
