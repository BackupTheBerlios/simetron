PREFIX= /usr

ASSEMBLIES= 

LIBRARY= ../lib/opengl.dll

all: $(LIBRARY)

$(LIBRARY): *.cs
	mcs --unsafe --target library $(ASSEMBLIES) --recurse '*.cs' -o $(LIBRARY)

install:
	install -o root -g root -m 644 $(LIBRARY) $(PREFIX)/lib

clean:
	rm -f $(LIBRARY)
	rm -rf generated
