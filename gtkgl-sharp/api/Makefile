PREFIX=/usr

API= \
	gtkgl-api.xml		\
	gdkgl-api.xml		\

INCLUDE_API=    \
	$(PREFIX)/share/gapi/art-api.xml                \
	$(PREFIX)/share/gapi/pango-api.xml              \
	$(PREFIX)/share/gapi/atk-api.xml                \
	$(PREFIX)/share/gapi/gnome-api.xml              \
	$(PREFIX)/share/gapi/gtk-api.xml                \
	$(PREFIX)/share/gapi/gdk-api.xml                \
	$(PREFIX)/share/gapi/gdk-symbols.xml

all: generated-stamp

generated-stamp: $(API)
	mono /usr/bin/gapi_codegen.exe --include $(INCLUDE_API) --generate $(API) && touch generated-stamp

clean:
	rm -f generated-stamp
