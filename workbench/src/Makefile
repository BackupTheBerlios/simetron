DIRS=Logging Data GUI
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

test: all
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done

clean:
	for i in $(DIRS) ; do \
		$(MAKE) -C $$i -f Makefile $@ || exit 1; \
	done

