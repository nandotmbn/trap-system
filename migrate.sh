#!/bin/bash

# Check if the correct number of arguments is provided
if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <version>"
    exit 1
fi

# Arguments
VERSION="$1"

cd '5_WebAPI'
dotnet ef migrations add $VERSION
dotnet ef database update
cd ..