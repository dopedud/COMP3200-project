# COMP3200 Part III Individual Project

This is a git repository for the module COMP3200 - Part III Individual Project. This unity 
project is about developing a machine learning model that act as a hostile entity towards
the player that can be seen in most horror games.

## Installation

This project uses Unity's ML Agents package cloned from the git repository https://github.com/Unity-Technologies/ml-agents/tree/release_20.

The steps to install and use ML agents in unity is sequenced below:

1. install ML agents package from unity's package manager
2. start command prompt from your folder
3. (IMPORTANT) for release 20, install python version 3.9.*, use "py --version" to check python version
(ALSO IMPORTANT) make sure to use py, or python, whichever corresponds to the python version you've installed
if the command that corresponds is py, then use that to do the next steps
4. create and activate a virtual environment using python in your unity's project file via cmd (venv\Scripts\activate)
5. pip install and upgrade the pip package (py -m pip install --upgrade pip)
6. pip install pytorch (for release 20, better to use from the docs)
7. pip install mlagents
8. rewind version protobuf package to 3.20.* (pip install protobuf==3.20.*)
9. (OPTIONAL) pip install packaging (if there's a ModuleNotFoundError)
10. test everything works by typing the command "mlagents-learn --help"

## Author

- Abeed (theabeed01@gmail.com)
