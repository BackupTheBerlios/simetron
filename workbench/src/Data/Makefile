#Settings
CSC = mcs
CSFLAGS = /debug /define:DEBUG /define:TRACE /t:library
CLRVM = mono --debug
INSTALL = /usr/bin/install
APP_NAME = Simetron.Data.dll
BIN_DIR = ../../bin
APP = $(BIN_DIR)/$(APP_NAME)

# 	Core/*.cs \

SOURCES = \
	./*.cs \
	NetworkTopology/*.cs \

REFERENCES = \
	/r:Simetron.Logging.dll

RESOURCES = \

all: $(APP)

$(APP): $(SOURCES)
	$(CSC) $(CSFLAGS) -L $(BIN_DIR) $(RESOURCES) $(REFERENCES) /out:$(BIN_DIR)/$(APP_NAME) $(SOURCES)

install: $(APP)
	if test x$$prefix = x; then \
		echo Usage is: make -f Makefile install prefix=YOURPREFIX; \
		exit 1; \
	fi;
	mkdir -p $(prefix)/bin
	$(INSTALL) -m 755 $(APP) $(PREFIX)/bin/$(APP_NAME)

clean:
	rm -f $(APP)

