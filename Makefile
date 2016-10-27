.PHONY : all clean mod dist

NAME = DraggableNavball
LIB = libs
CONF = build/PluginData/$(NAME)

all: mod
mod : build/$(NAME).dll $(CONF)/$(NAME).cfg

build/%.dll : src/%.cs
	@mkdir -p $(@D)
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(LIB) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.UI.dll

$(CONF)/%.cfg : src/%.cfg
	@mkdir -p $(@D)
	cp -f $< $@

dist : mod
	@mkdir -p dist
	ln -sfn ../build dist/$(NAME) && \
	cd dist && \
	zip -FSr $(NAME).zip $(NAME)

clean : 
	-rm -rf build
	-rm -rf dist
