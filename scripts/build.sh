#!/usr/bin/env bash
set -euo pipefail
dotnet restore TetrisCSharp.sln
dotnet build TetrisCSharp.sln -c Release
dotnet test TetrisCSharp.sln -c Release
