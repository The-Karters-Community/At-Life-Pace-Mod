# The Karters Modding Assistant - New Mod Template

This project will help your for building a brand new mod easily for the Prologue version of the game The Karters 2: Turbo-Charged. Stop asking or searching for tutorials, use this.

- (_Facultative but adviced_) Click on the "Use this template" button and choose "Create a new repository".
- Open your machine console and clone this project `git clone https://github.com/The-Karters-Community/TKMA-New-Mod-Template.git YourModName` or the project you just created if you followed the facultative step.
- Go to [The Karters Assistant SDK repository](https://github.com/The-Karters-Community/The-Karters-Modding-Assistant-SDK) and download its lastest release.
- Follow [its documentation](https://github.com/The-Karters-Community/The-Karters-Modding-Assistant-SDK/blob/main/doc/index.md).
- Replace **all occurences** of `NewModTemplate` you find: namespaces, class names, file names, etc.
- Run `dotnet restore`.
- Start developing your mod. ❤

## Testing your mod

In order to test your mod, you need to build it and to inject the generated file into the game.

- Open your machine console and move to the root directory of your mod. eg: `cd path/to/YourModName`.
- Build your mod: `dotnet build`.
- Copy the generated `YourModName.dll` file in `bin/net6.0/Debug/`.
- Paste it your game directory: `/Steam/steamapps/common/The Karters 2 Turbo Charged - Prologue/BepInEx/plugins/`
- Start the game! 🔥
