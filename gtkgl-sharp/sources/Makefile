DOWNLOADS = \
	ftp://ftp.linux.org.uk/mirrors/ftp.gnome.org/sources/gtkglarea/1.99/gtkglarea-1.99.0.tar.bz2 

all:	
	PERLLIB=/usr/share/perl5 gapi.pl gtkglarea-sharp.sources ../api/

get-source-code:
	for i in $(DOWNLOADS); do                          \
		wget $$i --output-document=- | tar -xj ;   \
	done;

distclean:
	for i in `ls`; do                     \
		if test -d $$i; then              \
			if test 'CVS' != $$i; then    \
				rm -Rf $$i;               \
			fi;                           \
		fi;                               \
	done;
