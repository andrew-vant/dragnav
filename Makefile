.PHONY : all clean

all : build/DraggableNavBall.dll

build/DraggableNavBall.dll : dragnavball.cs
	mcs $< -out:$@ \
		-target:library \
		-lib:libs \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.UI.dll

clean : 
	-rm build/* -v
