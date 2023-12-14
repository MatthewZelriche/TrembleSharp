#!/usr/bin/env python

import argparse
import subprocess
import os
import platform
import shutil

parser = argparse.ArgumentParser(description='Build Tremble.')
parser.add_argument('--config', dest='buildConfig',
                   default="Release",
                   help='The build type to use. \'Debug\' or \'Release\'')
parser.add_argument('--jobs', dest='jobs',
                   default="1",
                   help='The number of jobs to pass to the build system')

args = parser.parse_args()

subprocess.run(["cmake", "-Svendor/Tremble/", "-Bbuild/", "-DCMAKE_BUILD_TYPE=" + args.buildConfig])
subprocess.run(["cmake", "--build", "build", "--config", args.buildConfig, "-j" + args.jobs])
subprocess.run(["dotnet", "build", "src/src.csproj", "--output", "build/" + args.buildConfig])