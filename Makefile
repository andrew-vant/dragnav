NAME = DraggableControls
.PHONY : all mod meta dist release clean

LIB = libs
DOCS = $(wildcard *.md)
OUTDIR = out
BUILDDIR = $(OUTDIR)/$(NAME)
DATAPATH = PluginData/$(NAME)

.DELETE_ON_ERROR :

all : dist
mod : $(addprefix $(BUILDDIR)/, $(DOCS) $(NAME).dll $(DATAPATH)/$(NAME).cfg)
meta : $(OUTDIR)/ckan.version.json
dist : $(OUTDIR)/$(NAME).zip
release: clean meta dist

$(OUTDIR) $(BUILDDIR):
	mkdir -p $@

$(BUILDDIR)/%.dll : %.cs | $(BUILDDIR)
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(LIB) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.CoreModule \
		-reference:UnityEngine.UI.dll

$(BUILDDIR)/$(DATAPATH)/%.cfg : %.cfg | $(BUILDDIR)
	-mkdir -p $(@D)
	cp -f $< $@

$(BUILDDIR)/%.md : %.md | $(BUILDDIR)
	cp -f $< $@

# NOTE: meta.py returns nonzero if the build should not be released
# (e.g no version tag or dirty tree). Make will delete the file in that
# case per .DELETE_ON_ERROR above, to try to prevent zips with bogus version
# information from getting into the wild. To force it to keep the file,
# use `make -i meta`
$(BUILDDIR)/ckan.version.json : meta.py meta.yaml version.yaml.jinja | $(BUILDDIR)
	$< > $@

$(OUTDIR)/ckan.version.json : $(BUILDDIR)/ckan.version.json
	cp -vf $< $@
	#
# NOTE: the metadata file is intentionally not a prereq here, to allow
# test zips to be built. The meta file will get into the zip only if
# it's intentionally built first.
$(OUTDIR)/$(NAME).zip : mod
	cd $(OUTDIR) && zip -FSr $(NAME).zip $(NAME)


clean : 
	-rm -rvf $(OUTDIR)
