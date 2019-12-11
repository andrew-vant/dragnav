.PHONY : all clean mod dist

NAME = DraggableNavball
LIB = libs
CONF = build/PluginData/$(NAME)

all: mod
mod : build/$(NAME).dll build/README.md build/LICENSE.md build/CHANGES.md $(CONF)/$(NAME).cfg

build/%.dll : src/%.cs
	@mkdir -p $(@D)
	mcs $< \
		-target:library \
		-out:$@ \
		-lib:$(LIB) \
		-reference:Assembly-CSharp.dll \
		-reference:UnityEngine.dll \
		-reference:UnityEngine.CoreModule \
		-reference:UnityEngine.UI.dll

build/%.md : %.md
	@mkdir -p $(@D)
	cp -f $< $@

build/%.version : meta/meta.py meta/meta.yaml meta/version.json.jinja
	./meta/meta.py > $@

$(CONF)/%.cfg : src/%.cfg
	@mkdir -p $(@D)
	cp -f $< $@

release : mod build/$(NAME).version
	@mkdir -p dist
	ln -sfn ../build dist/$(NAME) && \
	cd dist && \
	zip -FSr $(NAME).zip $(NAME)
	@echo -n "\nRELEASE VERSION: "
	@grep ^version meta/meta.yaml | rev | cut -f 1 -d " " | rev
	@echo "MAKE SURE THIS IS CORRECT BEFORE DISTRIBUTING\n"

clean : 
	-rm -rf build
	-rm -rf dist
