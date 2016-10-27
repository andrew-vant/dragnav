.PHONY : all clean

libdir = libs
builddir = build

all : $(builddir)/DraggableNavBall.dll

build/DraggableNavBall.dll : dragnavball.cs
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(libdir) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.UI.dll

clean : 
	-rm $(builddir)/DraggableNavBall.dll
