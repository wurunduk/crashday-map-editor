# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

## [Unreleased]
### Added
- Camera is reset to the center of the map on map load.
- Enabled unity key select menu before start.
- Added a temp icon.
### Changed
- Dont load destroyed parts pf the meshes to avoid visual clutter bugs.
- Glass is now loaded as transparent texture (Fixes strange greehouse look, thanks Quruc90).
### Fixed
- Lamps dont create visual artifacts on ground anymore.
- Prevent loading maps when no map was selected in load map dialog.
- Camera does not create shadow artifacts now.

## [0.6.1] - 2018-08-09
### Added
- Added a changelog to the project.

### Changed
- Level has now three lamps with soft shadows enabled for easier sense of height.
