package com.olympum.cibeles.engine.core;
/*
 * Cibeles - Workflow Engine
 * 
 * Copyright (c) 2002-2003, The Olympum Group, http://www.olympum.com/
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer. 
 *
 * - Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation 
 * and/or other materials provided with the distribution. 
 *
 * - Neither the name of The Olympum Group nor the names of its contributors 
 * may be used to endorse or promote products derived from this software without 
 * specific prior written permission. 
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

import java.util.Arrays;

abstract class ExecutionObject {
	private final String name;
	private final String[] portNames;
	private StateEventListener listener;

	abstract void startExecution();

	ExecutionObject(String name, String[] portNames) {
		this.name = name;
		this.portNames = portNames;
		// sorting is necessary to use the binary search algorithm
		Arrays.sort(this.portNames);
		DataStore.allocate(this, portNames.length);
	}

	final void setListener(StateEventListener listener) {
		this.listener = listener;
	}

	final String getName() {
		return name;
	}

	final Object getPortValue(String portName) {
		int index = Arrays.binarySearch(portNames, portName);
		if (index >= 0) {
			return DataStore.getValue(this, index);
		} else {
			throw new RuntimeException(
				"Unknown port name: " + portName);
		}
	}

	final void setPortValue(String portName, Object portValue) {
		int index = Arrays.binarySearch(portNames, portName);
		if (index >= 0) {
			DataStore.setValue(this, index, portValue);
		} else {
			throw new RuntimeException(
				"Unknown port name: " + portName);
		}
	}

	final String[] getAllPortNames() {
		return portNames;
	}

	final boolean isNotRunning() {
		return this.getState() == State.NOT_RUNNING;
	}

	void reset() {
		DataStore.allocate(this, portNames.length);
	}

	final boolean contains(String portName) {
		return Arrays.binarySearch(portNames, portName) >= 0
			? true
			: false;
	}

	final State getState() {
		return DataStore.getState(this);
	}

	final void setState(State value) {
		State previous = DataStore.getState(this);
		DataStore.setState(this, value);
		if (listener != null) {
			StateEventInfo info =
				new StateEventInfo(previous, value);
			listener.onStateChange(this, info);
		}
	}

	public String toString() {
		StringBuffer sBuilder = new StringBuffer();
		sBuilder.append("\nObject Name=\t");
		sBuilder.append(this.getName());
		sBuilder.append("\t;State=\t");
		sBuilder.append(this.getState());
		sBuilder.append("\n\tPorts:");
		for (int i = portNames.length; i-- > 0;) {
			String portName = portNames[i];
			sBuilder.append("\n\t\tname=\t").append(
				portName).append(
				"\t;value=\t").append(
				getPortValue(portName));
		}
		return sBuilder.toString();
	}
}
