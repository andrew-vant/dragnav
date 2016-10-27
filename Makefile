.PHONY : all clean

libdir = libs
builddir = build
confdir = $(builddir)/PluginData/DraggableNavball

all : $(builddir)/DraggableNavball.dll $(confdir)/DraggableNavball.cfg

$(builddir) :
	mkdir -p $<

$(builddir)/DraggableNavball.dll : dragnavball.cs
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(libdir) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.UI.dll

$(confdir)/DraggableNavball.cfg : dragnavball.cfg
	mkdir -p $(confdir)
	cp -f $< $@
clean : 
	-rm -r $(builddir)/*
