# Cade's [Bopl Battle](https://store.steampowered.com/app/1686940/Bopl_Battle/) Mods

\*** Template/structure/original README created by [Almafa64](https://github.com/almafa64/almafa64-bopl-mods) \***

All of my mods for [Bopl Battle](https://store.steampowered.com/app/1686940/Bopl_Battle/) use [BepInEx](https://github.com/BepInEx/BepInEx).

- [Cade's Bopl Battle Mods](#Cades-bopl-battle-mods)
  - [Mods info](#mods-info)
  - [Installation](#installation)
<!--      - [After BepInEx installed](#after-bepinex-installed) -->
  - [Building](#building)

## Mods info
All mods need **BepInEx** Version **5.4.22**
- **BoplBattleTemplate**: Base of all my mods (created by [Almafa64](https://github.com/almafa64/almafa64-bopl-mods/tree/master/BoplBattleTemplate)). More advanced version of [shad0w_dev's](https://discord.com/channels/1175164882388275310/1177300281705365676/1177333041048334336) (thanks shad0w_dev)
- **CancelRock** (1.0.0): Allows you to cancel the rock ability by letting go of the ability button, instead of having to wait for the ability to finish.

## Installation
Need help to install BepInEx?<br>
Click [this link](https://docs.bepinex.dev/articles/user_guide/installation/index.html) to get started!

<!--
#### After BepInEx installed
1. Get mods from [release page](https://github.com/almafa64/almafa64-bopl-mods/releases)
2. Follow the Installation instruction on mod release page<br>
-->
## Building
1. Clone repo
1. Start setup.cmd and follow the instructions
1. **Important**: The solution uses DLLs from the **installed** BepInEx, so install it before step 5
1. Start MyMods.sln
1. Build the mod you would like
1. Mod DLL is at &lt;Mod name&gt;\\bin\\&lt;Release/Debug&gt;\\net46\\&lt;Mod name&gt;.dll (it will be copied to GameFolder\\BepInEx\\plugins\\&lt;Mod name&gt;)
