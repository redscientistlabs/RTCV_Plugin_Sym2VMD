# Sym2VMD
Plugin for generating VMDs in [RTCV](https://github.com/redscientistlabs/RTCV) based on linker/symbol maps.

Only works for GC/Wii linker maps built with Metrowerks.

# Usage

There are multiple generation modes:

- ByStaticLibrary generates vmds based on a block of defined symbols in each detected static library, or in symbols that have no static library, the source file, as that fills up the regex group that the static library would occupy.

- BySourceFile generates vmds similarly, but based on source files, provided the symbol corresponds to a static library.

- BySymbol generates vmds by each individual symbol.

As these generate vmds from the whole map, it will be very slow. However, you can filter by specific names corresponding to each mode. Each name filtered has to be seperated by a ``|``.

# Setup for building
You will need to reference DLLs from RTCV, which can be found in the releases or by building it yourself. This plugin is built with RTCV 5.1.0-b2 in mind.
