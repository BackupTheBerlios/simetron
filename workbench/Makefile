DIRS=src
DIST=simetron.bin-`date -u +%Y%m%d`
#MCS = mcs
INSTALL=/usr/bin/install
DOCFILES= README

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

dist: all
	mkdir $(DIST)
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile install prefix=$(PWD)/$(DIST) || exit 1; \
	done
	tar -c $(DIST) | gzip > $(DIST).tar.gz
	rm -rf $(DIST)

