# Destiny Collada Generator
 A program to generate Collada files of items from Destiny 2

## Features:
 This program is capable of generating a 3D model of any item available through Destiny 2's web/mobile api and exporting said model in Collada format, with all data intact. Items on this list will be checked off as they are implemented.
- [X] Core functions
	- [X] Export geometry
	- [X] Export mesh weights
	- [X] Export UV texture coordinates
	- [X] Export per-face vertex normals and tangents
	- [X] Export vertex colors
	- [X] Export dye slots for compatibility with D2 model rippers' shaders

- [X] API support
	- [X] Call files to convert from the API by item hash
	- [X] Convert files from the D1 API by item hash

- [ ] Additional features
	- [X] Generate textures for items in .png format
	- [ ] Generate a list of shader parameters for items, in an easy-to-understand layout
	
 ## Note about use on Linux:
 This tool requires GDI+ to function properly, which cannot be packaged with the Linux distribution of it. GDI+ can be installed on Linux by running the following commands:
```
sudo add-apt-repository ppa:quamotion/ppa
sudo apt-get update
sudo apt-get install -y libgdiplus
```

 ## Error reporting: 
 If the program exits without warning (command line closes on its own in the middle of converting), it's likely due to a bug or error. If this happens, create a new issue on the github repo, including the TGXM file or item hash that caused the crash. I'll push out an updated version of the program once the issue is fixed.
 
 ## Join the Destiny Model Rips Discord server!
 [The DMR Discord server](https://discord.gg/TsRah4t) has tutorials for working with the models produced by this tool, as well as a drive full of models already prepared for use and help channels for any issues you might run into while working with this tool or Destiny models in general. No commitment required, all of the models and tutorials are free to access.

 ## Code attributions:
 This program uses:

 Large sections of code adapted from [Lowlines' TGX loader for three.js](https://github.com/lowlines/destiny-tgx-loader), as well as their online manifest

 [Burtsev Alexey's .NET deep copy extension method](https://github.com/Burtsev-Alexey/net-object-deep-copy)

 [Alexandre Mutel's patch of the C# COLLADA specification classes](https://xoofx.com/blog/2010/08/24/import-and-export-3d-collada-files-with/)
