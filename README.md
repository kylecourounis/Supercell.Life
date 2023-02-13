# Supercell.Life
**Supercell.Life** is a private server, written by Kyle, for the dead Supercell game Smash Land, codenamed Life. 
By reverse engineering the app's binary using IDA, I have been working to recreate a server from the ashes of what was to be a great game if it had gone global. This project is placed under the GNU license. 

## The Story
This project began because I wanted to bring back one of Supercell's best games that had been killed off during its soft-launch in 2015. I loved this game so much, and I really didn't play it enough when it was available to the public. 

I aimed to use as much of Supercell's implementations and class and method namescheme as I could. 

For a list of features, you can visit [this page](https://kylecourounis.com/#smash-land) on my website. For a more actively updated list, go to the `Projects` tab in this repository. 

## Setup
***Please note that this project is not for beginners. You'll need at least some experience with Supercell private servers and side-loading iOS apps.***

To setup the project, you will need:
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [MongoDB](https://www.mongodb.com/try/download/community)

To build the project, run: 
```
mkdir Supercell.Life
git clone https://github.com/kyledoesode/Supercell.Life.git && cd Supercell.Life/Supercell.Life.Server
dotnet publish "Supercell.Life.Server.csproj" -c Release -o app
```

When you download and run the installer for MongoDB, make sure to check the box in the installer that tells it to install MongoDB Compass. MongoDB Compass is the GUI for MongoDB. Then, open MongoDB Compass and create a database called `SmashLand`. The server will automatically create the collections on its first run.

Here is a link to the [IPA file](https://mega.nz/file/2o0BxarL#v7ZMfta6IAfAxUWN_AUUNGYTZFuNM8RS2EW4d9-gEmM) that you will need to sideload onto your device. It works on 32-bit and 64-bit iOS devices on iOS 7 and above.

I recommend using [Sideloadly](https://sideloadly.io/) to sideload the IPA, as it is available for both Windows & Mac. However, you may use any method you feel comfortable with. One important thing to note is that if you are not using a paid Apple Developer account, you must change the bundle identifier of IPA in Sideloadly to something different. You can access this setting by clicking Advanced Options.

You must also be jailbroken in order to edit the hosts file to point the game to your computer's IP. (The original host is `game.smashlandgame.com`) 

Devices running iOS 12 and above have a tendency to ignore changes made to the hosts file. To remedy this, you must install a tweak Cydia called LetMeBlock. Just add PoomSmart's repository (https://poomsmart.github.io/repo/) as a source and you'll be able to install the tweak. 

### Other Credits
I have received great advice from my friends of many years in this [Discord](https://discord.gg/XdTw2PZ) server.

##### This project is not affiliated with Supercell, nor do I profit off of this project. No copyright infringment is intended.
