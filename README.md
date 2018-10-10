# Crashday 3D map editor
Crashday 3D editor made with unity.  
Current unity version: 2018.2.2f1

*Read this in other languages: [English](README.md), [Русский](README.ru.md).* 

## How to setup
1. [Download the latest release](https://github.com/wurunduk/crashday-map-editor/releases)
2. Unzip the editor into any folder
3. go to your crashday's data folder
 * steamapps/common/crashday/data/
4. You need to extract data000, 002, 005, 006 cpk's to this folder  
 (take the content folders from the dat---'s and put them in the Crashday/data folder)
5. Additinally you can install mods by finding their archives and unzipping them too (Highly untested!)
6. Launch the editor
7. First time editor will ask for the CD installation folder (the one with the .exe)

## Usage (buttons can be changed at the start in input tab)
### General
Hold middle mouse and drag to move around  
Shift+middle mouse to rotate view  
Scroll wheel to move forwards/backwards  
### Place tool
Left mouse click to place a tile  
Right mouse to rotate  
Left control to flip  
### Terrain edit tool
Right click to select points  
Right click + ctrl to deselect points  
Left click + drag up/dwon to lower or raise selected points  
A - deselect all points if selected, select all points if none are selected  

## Current TODO list
1. ~~Heightmaps loading~~(done)
 * Some tiles still have holes when connecting
2. ~~Texture loading~~(partialy done)
 * Needs UV loading
3. ~~Adding tile tools~~
4. ~~Adding terrain tools~~
5. Checkpoint tools
6. Object tools
7. Optimisations
 * Research C# collections to faster load maps (FindIndex takes too long when loading maps)
8. Better Mod tiles support
9. Fix stupid stuff
10. ~~Better controls~~
11. Move objects and CPs after map resize
 * CPs are moved now, but not tested for bugs

 ## Changelog
 You can view the changelog [here](https://github.com/wurunduk/crashday-map-editor/blob/master/CHANGELOG.md)


