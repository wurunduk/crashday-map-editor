# Crashday 3D map editor
Crashday 3D editor made with unity.  
Current unity version: 2018.1.0b13

## How to setup
1. go to your crashday's data folder
 * steamapps/common/crashday/data/
2. You need to extract data000, 002, 005, 006 cpk's to this folder  
 (take the content folders from the dat---'s and put them in the /data folder)
3. Additinally you can install mods by finding their archives and unzipping them too (Highly untested!)
4. Launch the editor
5. First time editor will ask for the CD installation folder (the one with the .exe)

## Usage
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
 * Needs correct UV loading
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


