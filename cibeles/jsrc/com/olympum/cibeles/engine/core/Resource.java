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

import java.lang.reflect.Constructor;
import java.lang.reflect.Member;
import java.lang.reflect.Method;
import java.util.HashMap;

import com.olympum.logging.Debug;

final class Resource {
	private ResourceCache rCache;

	Resource(ResourceCache rCache) {
		this.rCache = rCache;
	}

	// "instance" and "return" are two reserved keywords
	void run(HashMap data) {
		// we need to verify that data constains the same keys as parameters
		// the Resource expects
		int numParameters = rCache.size();
		/*
			if (numParameters != rCache.size()) {
			    throw new RuntimeException("The number of parameters does not match;" +
						       " required " + rCache.size() +
						       " but provided " + data.size());
						       }*/

		Object[] parameters = new Object[numParameters];
		for (int i = numParameters; i-- > 0;) {
			String paramName = rCache.getParameterName(i);
			if (data.containsKey(paramName)) {
				Object paramValue = data.get(paramName);
				Debug.writeLine(
					"Found parameter "
						+ paramName
						+ "("
						+ paramValue
						+ ")");
				parameters[i] = paramValue;
			} else {
				parameters = null;
				throw new RuntimeException(
					"Parameter "
						+ paramName
						+ " required but not found");
			}
		}

		// the hashtable may contain the instance on which to invoke
		// this method (for static and constructors this is ignored)
		Object instance = null;
		if (data.containsKey("_instance_")) {
			instance = data.get("_instance_");
			Debug.writeLine(
				"Found parameter instance (" + instance + ")");
		}

		Member info = rCache.getMemberInfo();
		Object returnValue = null;
		try {
			if (info instanceof Constructor) {
				Constructor cInfo = (Constructor) info;
				instance = cInfo.newInstance(parameters);
				Debug.writeLine(
					"Adding instance (" + instance + ")");
				data.put("_instance_", instance);
			} else if (info instanceof Method) {
				Method mInfo = (Method) info;
				returnValue =
					mInfo.invoke(instance, parameters);
				Debug.writeLine(
					"Adding return (" + returnValue + ")");
				data.put("_return_", returnValue);
			}
		} catch (Exception e) {
			e.printStackTrace();
			throw new RuntimeException(
				"Oops! Reflection exception",
				e);
		}
	}
}
