DIRS=sources api gdkgl gtkgl opengl sample
INSTALL=/usr/bin/install

default: all

all:
	@for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done

install:
	if test x$$prefix = x; then \
		echo Usage is: make -f Makefile install prefix=YOURPREFIX; \
		exit 1; \
	fi;
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done
	mkdir -p $(prefix)/share/doc/simetron
	$(INSTALL) -m 644 $(DOCFILES) $(prefix)/share/doc/simetron

test: all
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done

clean:
	-rm -f simetron.bin-*.tar.gz
	find . -name '*~' -exec rm -fr {} \;
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done
