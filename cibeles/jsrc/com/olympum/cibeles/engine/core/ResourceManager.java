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

import java.io.InputStream;
import java.util.HashMap;
import java.util.List;

import com.olympum.configuration.ConfigurationSettings;

import org.jdom.input.SAXBuilder;
import org.jdom.Document;
import org.jdom.Element;

final class ResourceManager {
	private final static ResourceManager theInstance =
		new ResourceManager();

	private HashMap resourcesCache;

	private static final String RESOURCES = "RESOURCES";
	private static final String RSR = "resource";
	private static final String RSR_ID = "id";
	private static final String RSR_MBR = "member";
	private static final String RSR_CLS = "class";
	private static final String RSR_MTH = "method";
	private static final String RSR_ARG = "arg";
	private static final String ARG_TYPE = "type";
	private static final String ARG_NAME = "name";

	static ResourceManager getInstance() {
		return theInstance;
	}

	Resource getResource(String resourceId) {
		// FIXME: NULL ? key not found?
		ResourceCache rCache =
			(ResourceCache) resourcesCache.get(resourceId);
		return new Resource(rCache);
	}

	boolean contains(String resourceId) {
		return resourcesCache.containsKey(resourceId);
	}

	boolean contains(String resourceId, String paramName) {
		if (!contains(resourceId)) {
			return false;
		}
		ResourceCache rCache =
			(ResourceCache) resourcesCache.get(resourceId);
		return rCache.contains(paramName);
	}

	private ResourceManager() {
		String xmlFilename =
			ConfigurationSettings.getAppSetting(RESOURCES);

		if (xmlFilename == null) {
			throw new RuntimeException(ErrorMessages.E0001);
		}

		InputStream is =
			Thread
				.currentThread()
				.getContextClassLoader()
				.getResourceAsStream(
				xmlFilename);

		if (is == null) {
			throw new RuntimeException(
				ErrorMessages.E0001 + "\t" + xmlFilename);
		}

		try {
			SAXBuilder builder = new SAXBuilder();
			Document doc = builder.build(is);
			is.close(); //why call something that does nothing?
			// Parse all processes
			Element e = doc.getRootElement();
			// create all the ResourceCache
			// this is a time consuming operation, as it involves a lot
			// of reflection, so do it only once
			resourcesCache = new HashMap();
			createResources(e);
		} catch (Throwable t) {
			t.printStackTrace();
		}
	}

	private void createResources(Element e) {
		List resTable = e.getChildren(RSR);
		for (int r = resTable.size(); r-- > 0;) {
			Element resRow = (Element) resTable.get(r);
			String id_ = resRow.getAttributeValue(RSR_ID);
			String member = resRow.getAttributeValue(RSR_MBR);
			String type = resRow.getAttributeValue(RSR_CLS);
			//method may be not present: handle this
			String method = resRow.getAttributeValue(RSR_MTH);
			//iterate through the arguments
			List argRows = resRow.getChildren(RSR_ARG);
			Element chid = resRow.getChild("arg");
			List arguments = resRow.getChildren("arg");
			String[] args = new String[argRows.size()];
			String[] names = new String[argRows.size()];
			for (int i = argRows.size(); i-- > 0;) {
				Element argRow = (Element) argRows.get(i);
				args[i] = argRow.getAttributeValue(ARG_TYPE);
				names[i] = argRow.getAttributeValue(ARG_NAME);
			}
			ResourceCache rCache =
				new ResourceCache(
					type,
					member,
					method,
					args,
					names);

			resourcesCache.put(id_, rCache);
		}
	}
}
