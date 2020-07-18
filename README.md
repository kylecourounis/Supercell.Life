# Supercell.Life
**Supercell.Life** is a private server, written by Kyle, for the dead Supercell game Smash Land, codenamed Life. 
By reverse engineering the app's binary using IDA, I have been working to recreate a server from the ashes of what was to be a great game if it had gone global. This project is placed under the GNU license. 

## The Story
This project began because I wanted to bring back one of Supercell's best games that had been killed off during its soft-launch in 2015. I loved this game so much, and I really didn't play it enough when it was available to the public. 

I aimed to use as much of Supercell's class and method namescheme as I could. *(I even recreated some base classes just to use their naming system. xD)*

This server is buggy in parts, but a very reasonable amount of stuff works. Battles do not work yet, as SectorState, the turn system, and the checksums are incomplete (I'll probably patch the binary to remove the checksum, but that's for another time). Quest completion is also finicky because you can go into a quest, exit out of the app, and it will count it as completed. Quest rewards are also not really working because dumb system I guess, lol. 

For more info on things that are and aren't working, go to the `Projects` tab.

## Setup
***Please note that this project is not for beginners. You need at least some experience in Supercell private servers and side-loading iOS apps.***

To setup the project, you will need:
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* MongoDB

To build the project, run: 
```
mkdir Supercell.Life
git clone https://github.com/kyledoesode/Supercell.Life.git && cd Supercell.Life/Supercell.Life.Server
dotnet publish "Supercell.Life.Server.csproj" -c Release -o app
```

To configure Mongo, download and run the installer, then create a database called `SmashLand`. The server will automatically create the collections on its first run.

Since Apple decided to remove 32-bit app support for iOS 11 and above, to use this server, you must have a 32-bit iOS device or a 64-bit device running iOS 10 or under. Since Cydia Impactor is broken, you'll need to use AltDeploy to install the [Smash Land IPA file](https://mega.nz/file/PlNlkCxa#-921zVTKWrTiWrkv4QQOX9Epl-6bX4aLZw3Qnz1gq9U). 

### Other Credits
I have received great advice from my friends of many years in this [Discord](https://discord.gg/XdTw2PZ) server.

##### This project is not affiliated with Supercell, and no copyright infringment is intended.