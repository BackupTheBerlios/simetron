#!/bin/sh
CIBELES_HOME=.

ANT_HOME=${CIBELES_HOME}/tools/ant

CP=${ANT_HOME}/lib/ant.jar
CP=${ANT_HOME}/lib/optional.jar:${CP}
CP=${CIBELES_HOME}/tools/javamake.jar:${CP}
CP=${CIBELES_HOME}/tools/junit.jar:${CP}
CP=${CIBELES_HOME}/lib/xmlParserAPIs.jar:${CP}
CP=${CIBELES_HOME}/lib/xercesImpl.jar:${CP}
CP=${CP}:${JAVA_HOME}/lib/tools.jar
CP=${CP}:${CLASSPATH}

ANT_OPTS="$ANT_OPTS -Dbuild.compiler=jikes"

BUILD_FILE="${CIBELES_HOME}/build.xml"

JAVACMD=${JAVA_HOME}/bin/java
"$JAVACMD" -classpath "${CP}" -Dant.home="${ANT_HOME}" $ANT_OPTS org.apache.tools.ant.Main -buildfile ${BUILD_FILE} $ANT_ARGS "$@"