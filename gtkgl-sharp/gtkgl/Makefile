PREFIX= /usr

ASSEMBLIES= \
	-r gtk-sharp.dll \
	-r glib-sharp.dll \
	-r gdk-sharp.dll \
	-r pango-sharp.dll \
	-r art-sharp.dll

LIBRARY= ../lib/gtkgl-sharp.dll

all: $(LIBRARY)

$(LIBRARY): generated/*.cs
	mcs --unsafe --target library $(ASSEMBLIES) --recurse '*.cs' -o $(LIBRARY)

install:
	install -o root -g root -m 644 $(LIBRARY) $(PREFIX)/lib

clean:
	rm -f $(LIBRARY)
	rm -rf generated
