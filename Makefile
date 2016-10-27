.PHONY : all clean

LIB = libs
BUILD = build/DraggableNavball
SOURCE = src
CONF = $(BUILD)/PluginData/DraggableNavball

all : $(BUILD)/DraggableNavball.dll $(CONF)/DraggableNavball.cfg

$(BUILD)/%.dll : $(SOURCE)/%.cs
	@mkdir -p $(@D)
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(LIB) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.UI.dll

$(CONF)/%.cfg : $(SOURCE)/%.cfg
	@mkdir -p $(@D)
	cp -f $< $@

clean : 
	-rm -rf build
