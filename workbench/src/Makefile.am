simetrondir=$(libdir)
simetron_DATA=Simetron.Logging.dll Simetron.Data.dll simetron.exe
CLEANFILES=Simetron.Logging.dll Simetron.Data.dll simetron.exe
CSC=mcs
CSFLAGS=/debug /define:DEBUG /define:TRACE
CLRVM=mono --debug

logging_sources= \
	$(srcdir)/Logging/*.cs 
logging_references = 
logging_resources = 

data_sources= \
	$(srcdir)/Data/*.cs \
	$(srcdir)/Data/NetworkTopology/*.cs
data_references =
data_resources = \
	/r:Simetron.Logging.dll

simetron_sources = \
	$(srcdir)/GUI/Commands/*.cs \
	$(srcdir)/GUI/Core/*.cs \
	$(srcdir)/GUI/Editors/*.cs \
	$(srcdir)/GUI/Workbench/*.cs \
	$(srcdir)/GUI/Logging/*.cs \
	$(srcdir)/GUI/Dialogs/*.cs 
simetron_references = \
	/r:glib-sharp.dll \
	/r:gdk-sharp.dll \
	/r:gtk-sharp.dll \
	/r:glade-sharp.dll \
	/r:Simetron.Logging.dll \
	/r:Simetron.Data.dll
simetron_resources = \
	/resource:$(srcdir)/GUI/glade/workbench.glade,workbench.glade \
	/resource:$(srcdir)/GUI/pixmaps/simetron-hicolor-48x48.png,simetron-hicolor-48x48.png \
	/resource:$(srcdir)/GUI/pixmaps/splash.jpg,splash.jpg

Simetron.Logging.dll: $(logging_sources)
	$(CSC) $(CSFLAGS) $(logging_resources) $(logging_references) /t:library /out:Simetron.Logging.dll $(logging_sources)

Simetron.Data.dll: $(data_sources) Simetron.Logging.dll
	$(CSC) $(CSFLAGS) $(data_resources) $(data_references) /t:library /out:Simetron.Data.dll $(data_sources)

simetron.exe: $(simetron_sources) $(srcdir)/GUI/glade/workbench.glade $(srcdir)/GUI/pixmaps/simetron-hicolor-48x48.png $(srcdir)/GUI/pixmaps/splash.jpg Simetron.Data.dll Simetron.Logging.dll
	$(CSC) $(CSFLAGS) $(simetron_resources) $(simetron_references) /out:simetron.exe $(simetron_sources)

run: simetron.exe
	$(CLRVM) simetron.exe
