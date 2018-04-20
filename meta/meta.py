#!/usr/bin/python3

import sys
from os.path import dirname, realpath

import yaml
import jinja2

# If at some point in the future I need more than one metadata file output,
# this should probably accept an argument specifying which one we're looking
# for.

def main():
    metadir = dirname(realpath(__file__))
    with open(metadir + "/meta.yaml") as f:
        meta = yaml.load(f)

    jloader = jinja2.FileSystemLoader(metadir)
    jenv = jinja2.Environment(loader=jloader, trim_blocks=True)
    print(jenv.get_template("version.json.jinja").render(**meta))

if __name__ == '__main__':
    main()
