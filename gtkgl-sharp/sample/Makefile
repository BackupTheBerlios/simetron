PREFIX= /usr

ASSEMBLIES= \
	-r System.Drawing.dll \
	-r gtk-sharp.dll \
	-r gdkgl-sharp.dll \
	-r gtkgl-sharp.dll \
	-r glib-sharp.dll \
	-r opengl.dll

SOURCES= \
	Simple.cs \
	Gears.exe

EXECUTABLE=../lib/Simple.exe \
	../lib/Gears.exe

all:

Simple.exe: Simple.cs
	mcs -L ../lib/ $(ASSEMBLIES) Simple.cs

Gears.exe: Gears.cs
	mcs -L ../lib $(ASSEMBLIES) Gears.cs

install:
	install -o root -g root -m 644 $(EXECUTABLE) $(PREFIX)/lib

clean:
	rm -f $(EXECUTABLE)

run: $(EXECUTABLE)
	mono $(EXECUTABLE)
