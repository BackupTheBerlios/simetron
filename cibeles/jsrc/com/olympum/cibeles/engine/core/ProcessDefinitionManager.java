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
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import com.olympum.logging.Trace;
import com.olympum.configuration.ConfigurationSettings;

import org.jdom.input.SAXBuilder;
import org.jdom.Document;
import org.jdom.Element;

final class ProcessDefinitionManager {
	private final static String WFDL = "WFDL";
	private final static String PROC = "process";
	private final static String PROC_ID = "id";
	private final static String PROC_ATRS = "attributes";
	private final static String ATRS_ATR = "attribute";
	private final static String ATR_ID = "id";
	private final static String PROC_ACTS = "activities";
	private final static String ACTS_ACT = "activity";
	private final static String ACT_ID = "id";
	private final static String ACT_PRMS = "parameters";
	private final static String PRMS_PRM = "parameter";
	private final static String PRM_ID = "id";
	private final static String ACT_IMP = "implementation";
	private final static String IMP_REFID = "refId";
	private final static String IMP_MAP = "map";
	private final static String MAP_ACT = "activity";
	private final static String MAP_RSR = "resource";
	private final static String PROC_TRNS = "transitions";
	private final static String TRNS_TRN = "transition";
	private final static String TRN_ID = "id";
	private final static String TRN_FOBJ = "fromObject";
	private final static String TRN_TOBJ = "toObject";
	private final static String TRN_CONN = "connection";
	private final static String CONN_FPRM = "fromParameter";
	private final static String CONN_TPRM = "toParameter";

	private final static ProcessDefinitionManager singleton =
		new ProcessDefinitionManager();
	private HashMap processes;
	private ResourceManager rm;

	private ProcessDefinitionManager() {
		// Create a dataset where to store the process 
		// definitions.
		processes = new HashMap();
		rm = ResourceManager.getInstance();
		try {
			init();
		} catch (RuntimeException e) {
			Trace.writeLine(e.getMessage());
			Trace.writeLine(e.getStackTrace());
			Throwable inner = e.getCause();
			if (inner != null) {
				Trace.writeLine(inner.getMessage());
				Trace.writeLine(inner.getStackTrace());
			}
			throw e;
		}
	}

	static ProcessDefinitionManager getInstance() {
		return singleton;
	}

	Process getProcess(String processName) {
		if (processes.containsKey(processName)) {
			return (Process) processes.get(processName);
		} else {
			throw new RuntimeException(
				ErrorMessages.E0003 + ":\t" + processName);
		}
	}

	private void init() {
		// the file that contains the process definitions
		// FIXME: replace this with a configurable location and 
		// add support for multiple process definition files
		// The best way to do this is to have an include tag 
		// and reference the other files that contain definitions.
		// Maybe also support for directories.
		// This has to be done in sync with auto refresh ...
		String xmlFilename = ConfigurationSettings.getAppSetting(WFDL);

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
			createProcesses(e);
		} catch (Throwable t) {
			t.printStackTrace();
		}
	}

	private void createProcesses(Element processDefinitions) {
		List processTable = processDefinitions.getChildren(PROC);
		declareProcesses(processTable);
		declareActivities(processTable);
		declareTransitions(processTable);
	}

	private void declareProcesses(List processTable) {
		for (int i = processTable.size(); i-- > 0;) {
			Element procRow = (Element) processTable.get(i);
			Process p = declareProcess(procRow);
			this.processes.put(p.getName(), p);
		}
	}

	private Process declareProcess(Element procRow) {
		String processName = procRow.getAttributeValue(PROC_ID);
		//get all rows in the attributes table(only one row)
		//owned by this process
		List attributes = procRow.getChildren(PROC_ATRS);
		List attRows =
			((Element) attributes.get(0)).getChildren(ATRS_ATR);
		List procPortNames = new ArrayList(attRows.size());
		//iterate through all the attributes
		for (int iPPort = attRows.size(); iPPort-- > 0;) {
			Element attRow = (Element) attRows.get(iPPort);
			String portName = attRow.getAttributeValue(ATR_ID);
			if (procPortNames.contains(portName)) {
				//ERROR: port name must be unique
				String message =
					ErrorMessages.E0004 + ":\t" + portName;
				throw new RuntimeException(message);
			}
			procPortNames.add(portName);
		}
		String[] ppNames = new String[procPortNames.size()];
		procPortNames.toArray(ppNames);
		Process p = new Process(processName, ppNames);
		return p;
	}

	private void declareActivities(List processTable) {
		for (int pIndex = processTable.size(); pIndex-- > 0;) {
			Element procRow = (Element) processTable.get(pIndex);
			String processName = procRow.getAttributeValue(PROC_ID);
			Process p = (Process) processes.get(processName);
			//get the row in the activities table 
			//owned by this process
			List activities = procRow.getChildren(PROC_ACTS);
			//and iterate through all the activities
			List acts =
				((Element) activities.get(0)).getChildren(
					ACTS_ACT);
			for (int aIndex = acts.size(); aIndex-- > 0;) {
				Element actRow = (Element) acts.get(aIndex);
				Activity a = declareActivity(actRow, p);
				p.addExecutionObject(a);
			}
		}
	}

	private Activity declareActivity(Element actRow, Process p) {
		String act_id = (String) actRow.getAttributeValue(ACT_ID);
		//get all the children parameters
		List parameters = actRow.getChildren(ACT_PRMS);
		//there should be only one entry
		List paramRows =
			((Element) parameters.get(0)).getChildren(PRMS_PRM);
		//use an array insted of an arraylist
		ArrayList actPorts = new ArrayList(paramRows.size());
		for (int iAPort = paramRows.size(); iAPort-- > 0;) {
			Element paramRow = (Element) paramRows.get(iAPort);
			String paramName = paramRow.getAttributeValue(PRM_ID);
			if (actPorts.contains(paramName)) {
				//ERROR: port name must be unique
				String message =
					ErrorMessages.E0005 + ":\t" + paramName;
				throw new RuntimeException(message);
			}
			actPorts.add(paramName);
		}
		//get the row in the implementation table owned by this
		//activity
		List implementations = actRow.getChildren(ACT_IMP);
		//there should be only one implementation row
		Element impRow = (Element) implementations.get(0);
		String resName = impRow.getAttributeValue(IMP_REFID);

		if (!rm.contains(resName)) {
			//ERROR: resource must exist
			String message = ErrorMessages.E0007 + ":\t" + resName;
			throw new RuntimeException(message);
		}
		List mappingRows = impRow.getChildren(IMP_MAP);
		Mapping[] mappings = new Mapping[mappingRows.size()];
		int iMap = 0;
		//add mappings to the implementation
		for (int im = mappingRows.size(); im-- > 0;) {
			Element mappingRow = (Element) mappingRows.get(im);
			String actPrtName =
				mappingRow.getAttributeValue(MAP_ACT);
			String rsrPrtName =
				mappingRow.getAttributeValue(MAP_RSR);
			//check if activity port exists
			if (!actPorts.contains(actPrtName)) {
				//ERROR: activity port must exist
				String message =
					ErrorMessages.E0006
						+ ":\t"
						+ actPrtName;
				throw new RuntimeException(message);
			}
			//check if the resource port exists
			if (!rm.contains(resName, rsrPrtName)) {
				//ERROR: resource port must exist
				String message =
					ErrorMessages.E0008
						+ ":\t"
						+ resName
						+ "/"
						+ rsrPrtName;
				throw new RuntimeException(message);
			}
			mappings[iMap++] = new Mapping(actPrtName, rsrPrtName);
		}
		String[] sActPorts = new String[actPorts.size()];
		actPorts.toArray(sActPorts);
		Activity a =
			new Activity(act_id, sActPorts, resName, mappings, p);
		return a;
	}

	private void declareTransitions(List processTable) {
		for (int pr = processTable.size(); pr-- > 0;) {
			Element procRow = (Element) processTable.get(pr);
			String processName = procRow.getAttributeValue(PROC_ID);
			Process p = (Process) processes.get(processName);
			//find all the transitions
			List transitions = procRow.getChildren(PROC_TRNS);
			List transRows =
				((Element) transitions.get(0)).getChildren(
					TRNS_TRN);
			for (int tr = transRows.size(); tr-- > 0;) {
				Element transRow = (Element) transRows.get(tr);
				Transition t = declareTransition(transRow, p);
				p.addTransition(t);
			}
		}
	}

	private Transition declareTransition(Element transRow, Process p) {
		String id = transRow.getAttributeValue(TRN_ID);
		String sFromObject = transRow.getAttributeValue(TRN_FOBJ);
		ExecutionObject fromObject = p.getExecutionObject(sFromObject);
		if (fromObject == null) {
			// the fromObject is not an activity, 
			// it must be a process
			fromObject =
				(ExecutionObject) processes.get(sFromObject);
			//ERROR: fromObject cannot be null
			if (fromObject == null) {
				String message =
					ErrorMessages.E0009
						+ ":\t"
						+ sFromObject;
				throw new RuntimeException(message);
			}
			if (fromObject != p) {
				p.addExecutionObject(fromObject);
			}
		}
		String sToObject = transRow.getAttributeValue(TRN_TOBJ);
		ExecutionObject toObject = p.getExecutionObject(sToObject);
		if (toObject == null) {
			toObject = (ExecutionObject) processes.get(sToObject);
			//ERROR: toObject cannot be null
			if (toObject == null) {
				String message =
					ErrorMessages.E0009 + ":\t" + sToObject;
				throw new RuntimeException(message);
			}

			if (toObject != p) {
				p.addExecutionObject(toObject);
			}
		}
		//iterate through all the connections
		List connRows = transRow.getChildren(TRN_CONN);
		Connection[] connections = new Connection[connRows.size()];
		for (int iConn = connRows.size(); iConn-- > 0;) {
			Element connRow = (Element) connRows.get(iConn);
			String fromParam = connRow.getAttributeValue(CONN_FPRM);
			if (!fromObject.contains(fromParam)) {
				//ERROR: fromParam must exist in fromObject
				String message =
					ErrorMessages.E0010
						+ ":\t"
						+ fromParam
						+ "("
						+ fromObject.getName()
						+ ")";

			}
			String toParam = connRow.getAttributeValue(CONN_TPRM);
			if (!toObject.contains(toParam)) {
				//ERROR: fromParam must exist in fromObject
				String message =
					ErrorMessages.E0010
						+ ":\t"
						+ toParam
						+ "("
						+ toObject.getName()
						+ ")";

			}
			connections[iConn] = new Connection(fromParam, toParam);
		}
		Transition t =
			new Transition(fromObject, toObject, connections);
		return t;
	}
}
