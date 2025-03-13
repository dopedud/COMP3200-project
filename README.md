# COMP3200 Part III Individual Project

## About

This is a git repository for the module COMP3200 - Part III Individual Project. This unity 
project is about developing a machine learning model that act as a hostile entity towards
the player that can be seen in most horror games.

## Video Demonstration

<video src = "demo.mp4" width = "540" controls></video>

## Installation and Usage

This project uses Unity's ML Agents package cloned from the git repository https://github.com/Unity-Technologies/ml-agents/tree/release_20.

The steps to install and use ML agents in unity is sequenced below:

1. install ML agents package from unity's package manager
2. start command prompt from your folder
3. (IMPORTANT) for release 20, install python version 3.9.*, enter `py --version` into cmd to check python version
(ALSO IMPORTANT) make sure to use `py`, or `python`, whichever corresponds to the python version you've installed
if the command that corresponds is `py`, then use that to do the next steps
4. create a virtual environment using python in your unity's project file by typing `py -m venv venv` into cmd
5. pip install and upgrade the pip package by typing `py -m pip install --upgrade pip` into cmd
6. `pip install pytorch` (for release 20, better to use from the docs) in cmd
7. `pip install mlagents` in cmd
8. rewind protobuf version package to 3.20.* by typing `pip install protobuf==3.20.*` into cmd
9. (OPTIONAL) `pip install packaging` in cmd (if there's a `ModuleNotFoundError`)
10. test everything works by typing the command `mlagents-learn --help` in cmd

## Activation and Training

There are 2 ways to train the ML model; via unity editor or creating a build and run from that.

Before running training, the pre-requisite steps below are required to run to allow communication between the Python learner and the unity editor:

1. on your projects root folder, type `cmd` in the folder directory bar
2. activate python environment by typing `venv\Scripts\activate` into cmd
3. type `mlagents-learn` to put the python learner into standby, you can use additional parameters like `--force` to overwrite previous training data, `--resume` to resume previous training data, and `--run-id=<your run id>` to specify a different training session, and also if you have custom configuration for training, do `config\<your config>.yaml` after `mlagents-learn` into cmd

### Via Unity Editor

1. (assuming the environment is set up, and do this before running the pre-requisite steps) open unity editor
2. enter play mode in unity editor
3. (IMPORTANT) keep the unity editor window active to allow training to progress

### Via Running a Seperate Build

1. (do this before running the pre-requisite steps) create a build of your training environment
2. (IMPORTANT) make sure that the build consist only the ML agent and the training environment, and any other gameplay aspects in your build should be excluded
3. for step 3 of running the pre-requisite steps, important additional parameters you can use are `--env=Builds\ChasePlayer-Basic\COMP3200-project` that determines where to locate and run the build (in this case it's in the `Builds\ChasePlayer-Basic` folder named `COMP3200-project`), `--num-envs=1` that determines how many number of seperate environments that should be run (in this case is 1, although higher numbers lead to higher usage of computational resources), and `--no-graphics` that runs the build without rendering the game
4. (IMPORTANT) if your game or ML agent uses visual data to train, you might not want to use `--no-graphics` parameter as that will prevent the agent from training

Once training is done, import `.onnx` file from `results\ppo` folder into `Assets` folder from your projects root folder.
Navigate your `Behaviour Parameters` component in unity editor and drag and drop the `.onnx` file into the `Model` field (or choose the `.onnx` file by browsing the file via clicking the circle at the right side of the field bar).

### Visualising

To visualise learning data, do the following steps:

1. on your projects root folder, type `cmd` in the folder directory bar
2. activate python environment by typing `venv\Scripts\activate` into cmd
3. type `tensorboard --logdir results` into cmd, it will print a message stating the URL of the website where it shows the learning data (usually it's `http://localhost:6006/`), go to this URL in any web browser to view the visualisation of training data

### Common Commands

These are the common commands you would want to use when training, for easy copy and paste into cmd:
- `mlagents-learn config\ChasePlayer-1M.yaml --run-id=ChasePlayer-Basic --force --env=Builds\ChasePlayer-Basic\COMP3200-project --num-envs=1 --no-graphics`
- `mlagents-learn config\ChasePlayer-1M.yaml --run-id=ChasePlayer-Basic2 --initialize-from=ChasePlayer-Basic --force --env=Builds\ChasePlayer-Basic2\COMP3200-project --num-envs=1 --no-graphics`
- `mlagents-learn config\ChasePlayer-Memory-3M.yaml --run-id=ChasePlayer-Level1 --initialize-from=ChasePlayer-Intermediate2 --force --env=Builds\ChasePlayer-Level1\COMP3200-project --num-envs=1 --no-graphics`

### UNITY VERSION - 2022.3.13f1
