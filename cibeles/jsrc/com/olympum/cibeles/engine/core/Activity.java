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

import com.olympum.logging.Debug;

final class Activity extends ExecutionObject {
	private Mapping[] mappings;
	private String resourceName;
	private ResourceManager rm;

	Activity(
		String name,
		String[] portNames,
		String resourceName,
		Mapping[] mappings,
		Process parent) {
		super(name, portNames);
		setListener(parent);
		this.resourceName = resourceName;
		this.mappings = mappings;
		this.rm = ResourceManager.getInstance();
	}

	void startExecution() {
		// change the state and notify the listener
		setState(State.RUNNING);

		// the map used to provide the data to the resource
		HashMap inData = new HashMap();

		// map the data from this activity to the resource
		Mapping mapping;
		for (int i = mappings.length; i-- > 0; ) {
			mapping = mappings[i];
			String paramName = mapping.getParameterName();
			String portName = mapping.getPortName();
			Object portValue = getPortValue(mapping.getPortName());
			Debug.writeLine(
				"Mapping port "
					+ portName
					+ "("
					+ portValue
					+ ") to parameter "
					+ paramName);
			inData.put(paramName, portValue);
		}

		Resource resource = rm.getResource(resourceName);
		resource.run(inData);

		// map the data from the resource to the activity
		for (int i = mappings.length; i-- > 0; ) {
			mapping = mappings[i];
			String portName = mapping.getPortName();
			String parameterName = mapping.getParameterName();
			Object parameterValue = null;
			if (inData.containsKey(parameterName)) {
				parameterValue = inData.get(parameterName);
				Debug.writeLine(
					"Mapping parameter "
						+ parameterName
						+ "("
						+ parameterValue
						+ ") to port "
						+ portName);
			}
			setPortValue(portName, parameterValue);
		}

		// notify the observer that we are done
		setState(State.COMPLETED);
	}
}
