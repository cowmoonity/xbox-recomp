# Supercow XBOX Recompiled

| [**RU**](./README_RU.md) |
|--------------------------|

## About
Full recompilation of the SuperCow game, specifically, XBOX 360 version. The project is working on **[FNA Framework](https://github.com/FNA-XNA/FNA)**, since it's a 1:1 alternative for **XNA Framework** needed for the game.

### Important
This project by any means is not recompilation of PC version, nor mobile version, because XBOX version was created specifically for XBOX 360 and rewritten from ground up on `C#`, not `C++` like other versions. Moreover, it was made by different game studio.

Also, the project does not fix any original bugs, doesn't rewrite game to be similar to PC/mobile version, doesn't add keyboard support, doesn't work with uncompressed game assets and doesn't fix save files. For all of that there will be fork of this project (coming soon).

## Build
To build this project you need:

- **.NET Framework 4.8** - in theory, 4.0 works fine too, but was not tested
- **C# 10.0** or higher
- **[FNA Framework](https://github.com/FNA-XNA/FNA)** repo as linked project
- **[FNA.NetStub](https://github.com/FNA-XNA/FNA.NetStub)** - imitates XBOX Live API
- `FAudio`, `FNA3D` and `SDL3` libraries, simply download them from original **[fnalibs-daily](https://github.com/FNA-XNA/fnalibs-dailies)** repo and place in `libs\` folder before build

### Game Assets
Because of legal concerns, project does not contain game assets such as textures, models, and sounds. They're still needed for the game to work and can be placed in `content\` folder.

## Contacts
- [SuperCow Cowmoonity discord](discord.supercow.community)

#### P.s Project was developed by one and only Human Intelligence