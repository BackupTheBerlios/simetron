#Settings
CSC = mcs
CSFLAGS = /debug /define:DEBUG /define:TRACE
CLRVM = mono --debug
INSTALL = /usr/bin/install
APP_NAME = GSimetron.exe
BIN_DIR = ../../bin
APP = $(BIN_DIR)/$(APP_NAME)

SOURCES = \
	Commands/*.cs \
	Core/*.cs \
	Editors/*.cs \
	Workbench/*.cs \
	Logging/*.cs \
	Dialogs/*.cs 

REFERENCES = \
	/r:glib-sharp.dll \
	/r:gdk-sharp.dll \
	/r:gtk-sharp.dll \
	/r:glade-sharp.dll \
	/r:Simetron.Data.dll \
	/r:Simetron.Logging.dll 


RESOURCES = \
	/resource:glade/workbench.glade \
	/resource:pixmaps/simetron-hicolor-48x48.png \
	/resource:pixmaps/splash.jpg

all: $(APP)

$(APP): $(SOURCES) glade/workbench.glade
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

run: $(APP)
	@$(CLRVM) $(APP)
