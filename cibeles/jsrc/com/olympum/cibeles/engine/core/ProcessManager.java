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

import java.util.HashMap;

import com.olympum.cibeles.engine.core.Process;

public final class ProcessManager {
	private final static ProcessDefinitionManager pdm =
		ProcessDefinitionManager.getInstance();

	private ProcessManager() {
	}

	public static HashMap run(String processName, HashMap input) {
		Process p = createProcess(processName);
		setInputData(p, input);
		p.startExecution();
		HashMap output = getOutputData(p);
		p.reset();
		return output;
	}

	private static Process createProcess(String name) {
		//obtain the definition for the required process
		Process p = pdm.getProcess(name);
		return p;
	}

	private static void setInputData(Process process, HashMap data) {
		// find the in ports for this process
		String[] portNames = process.getAllPortNames();
		String portName;
		for (int i = portNames.length; i-- > 0;) {
			portName = portNames[i];
			process.setPortValue(portName, data.get(portName));
		}
	}

	private static HashMap getOutputData(Process process) {
		HashMap data = new HashMap();
		String[] portNames = process.getAllPortNames();
		String portName;
		for (int i = portNames.length; i-- > 0;) {
			portName = portNames[i];
			data.put(portName, process.getPortValue(portName));
		}
		return data;
	}
}
