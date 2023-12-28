# Wow Config Copier

## Table of Contents
- [About the Project](#about-the-project)
    - [Built With](#built-with)
- [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Building From Source](#building-from-source)
    - [Running](#running)
    - [Running From Source](#running-from-source)
    - [Running From Release](#running-from-release)
- [Usage](#usage)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Acknowledgements](#acknowledgements)

## About The Project

The World of Warcraft Config Copier is a tool designed to address the issue of transferring configuration files between accounts. This project was started to fill the gap left by the lack of built-in features for ease of transferring configuration files between accounts. The goal of this project is to provide a simple and easy to use tool for transferring configuration files between accounts.

World of Warcraft Config Copier offers a straightforward and easy to use interface for copying configuration files between accounts. The tool is designed for minimal user input just select the account to want to copy from and the account you want to copy to and click copy. The tool will then copy the configuration files from the selected account to the selected account. The tool is designed with backups in mind they are saved in 
``%appdata%\WowConfigCopySettings\Backups``

Key Features:
- Quick and Easy Setup: Just download the release and run the executable, the project is designed to be a portable executable.
- Backups: The tool has the ability to backup the configuration files.
- Easy to Use: The tool is designed to be easy to use with minimal user input.

### Built With

- [.NET Core](https://dotnet.microsoft.com/)
- [Prism](https://prismlibrary.com/)

## Getting Started

### Prerequisites

- [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Rider](https://www.jetbrains.com/rider/)

### Building From Source


1. Clone the repo
```sh
git clone https://github.com/ErrorDodo/WowConfigCopy.git
```
2. Open the solution in Visual Studio 2022 or Rider
3. Build the solution
4. Run the solution

### Running

Depending on if you are running the project from source or from a release the steps will be different.

#### Running From Source

1. Open the solution in Visual Studio 2022 or Rider
2. Build the solution
3. Run the solution

#### Running From Release

1. Download the latest release from [here](https://github.com/ErrorDodo/WowConfigCopy/releases)
2. Extract the zip file
3. Run the executable


## Usage

1. Select the Realm that the account is from
2. Select the account that you want to have the configuration files copied to
3. Click the copy files button
4. Select the account you want to copy from
5. Follow the popup instructions
6. Wait for the files to be copied


## Roadmap

See the [open issues](https://github.com/ErrorDodo/WowConfigCopy/issues) for a list of proposed features (and known issues).

## Contributing

Any contributions you make are greatly appreciated. You can view what needs to be done in the [issues](https://github.com/ErrorDodo/WowConfigCopy/issues) section.
To contribute to the project follow these steps:

1. Fork the Project
2. Create your Feature Branch based on the develop branch (`git checkout -b feature/NewFeature`)
3. Commit your Changes (`git commit -m 'Add some NewFeature'`)
4. Push to the Branch (`git push origin feature/NewFeature`)
5. Open a Pull Request to the [develop](https://github.com/ErrorDodo/WowConfigCopy/tree/develop) branch
6. Wait for the pull request to be reviewed
7. Once the pull request has been merged after review and testing it will be added to the next release

## License

Distributed under the GNU Public License v3.0. See [LICENSE](./LICENSE) for more information.

## Contact

## Acknowledgements

If you're after a WowAddon that does the same thing check this out [MySlot](https://www.curseforge.com/wow/addons/myslot)