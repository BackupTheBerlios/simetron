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

import java.lang.reflect.Member;
import java.util.Arrays;

final class ResourceCache {
	private Class type;
	private Member memberInfo;
	private String[] paramNames;
	private String[] sortedParamNames;

	ResourceCache(
		String type,
		String member,
		String method,
		String[] paramTypeNames,
		String[] paramNames) {
		try {
			this.type =
				Class.forName(
					type,
					true,
					Thread
						.currentThread()
						.getContextClassLoader());

			// TODO: handle arrays (Array.newInstance(..))
			Class[] paramTypes = new Class[paramTypeNames.length];
			for (int i = paramTypeNames.length; i-- > 0;) {
				paramTypes[i] =
					getClassForName(paramTypeNames[i]);
			}

			Member info;
			// create the member info
			if (member.equals("constructor")) {
				info = this.type.getConstructor(paramTypes);
			} else if (member.equals("method")) {
				info = this.type.getMethod(method, paramTypes);
			} else if (member.equals("static")) {
				info = this.type.getMethod(method, paramTypes);
			} else {
				info = null;
				// TODO: add fields
				// in the current implementation the operation/member can only
				// be a constructor or a method. in future releases we should accept
				// fields
			}

			this.memberInfo = info;

			this.paramNames = paramNames;
			// in order to be able to use Arrays.binarySearch, we 
			// need to have a sorted array. however we don't want
			// to modify the order, as order matters while invoking a
			// reflected member
			// we use a clone just for this purpose
			this.sortedParamNames = (String[]) paramNames.clone();
			Arrays.sort(this.sortedParamNames);
		} catch (Exception e) {
			throw new RuntimeException(
				"Oops! Reflection exception",
				e);
		}
	}

	Class getType() {
		return type;
	}

	Member getMemberInfo() {
		return memberInfo;
	}

	String getParameterName(int index) {
		return paramNames[index];
	}

	int size() {
		return paramNames.length;
	}

	boolean contains(String paramName) {
		// two reserved keywords
		if (paramName.equals("_return_")
			|| paramName.equals("_instance_")) {
			return true;
		}

		return Arrays.binarySearch(sortedParamNames, paramName) >= 0
			? true
			: false;
	}

	private final static Class getClassForName(String name) {
		Class clazz = null;
		try {
			// name can either be:
			// a. a fully qualified Class name
			// b. an array type
			// c. a primitive Java type
			if (name.equals("boolean")) {
				clazz = Boolean.TYPE;
			} else if (name.equals("byte")) {
				clazz = Byte.TYPE;
			} else if (name.equals("char")) {
				clazz = Character.TYPE;
			} else if (name.equals("short")) {
				clazz = Short.TYPE;
			} else if (name.equals("int")) {
				clazz = Integer.TYPE;
			} else if (name.equals("long")) {
				clazz = Long.TYPE;
			} else if (name.equals("float")) {
				clazz = Float.TYPE;
			} else if (name.equals("double")) {
				clazz = Double.TYPE;
			} else {
				clazz =
					Class.forName(
						name,
						true,
						Thread
							.currentThread()
							.getContextClassLoader());
			}
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
		return clazz;
	}
}
