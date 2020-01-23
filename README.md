# Destiny Collada Generator
 A program to generate Collada files of items from Destiny 2

## Features:
 This program will, when complete, be capable of generating a 3D model of any item available through Destiny 2's web/mobile api and exporting said model in Collada format, with all data intact. Items on this list will be checked off as they are implemented.
- [X] Core functions
	- [x] Export geometry
	- [X] Export mesh weights
	- [x] Export UV texture coordinates - still buggy
	- [X] Export per-face vertex normals and tangents
	- [x] Export vertex colors
	- [X] Export dye slots for compatibility with D2 model rippers' shaders

- [ ] API support
	- [ ] Cache a copy of the D2 asset manifest to reduce API calls, with function to update manifest
	- [ ] Search cached manifest for items by name
	- [X] Call files to convert from the API by item hash

- [ ] Additional features (lowest priority)
	- [ ] Generate textures for items in either .png or .dds format
	- [ ] Generate a list of shader parameters for items, in an easy-to-understand layout

 ## Error reporting: 
 If the program exits without warning (command line closes on its own in the middle of converting), it's likely due to a bug or error. If this happens, create a new issue on the github repo, including the TGXM file or item hash that caused the crash. I'll push out an updated version of the program once the issue is fixed.
 
 ## Code attributions:
 This program uses:

 Large sections of code adapted from [Lowlines' TGX loader for three.js](https://github.com/lowlines/destiny-tgx-loader)

 [Burtsev Alexey's .NET deep copy extension method](https://github.com/Burtsev-Alexey/net-object-deep-copy)

 [Alexandre Mutel's patch of the C# COLLADA specification classes](https://xoofx.com/blog/2010/08/24/import-and-export-3d-collada-files-with/)
