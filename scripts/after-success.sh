#!/bin/bash
echo Executing after success scripts on branch $TRAVIS_BRANCH
echo Creating Collectively.Services.Storage.Models NuGet package
./scripts/dotnet-pack.sh
echo Publishing application
./scripts/dotnet-publish.sh
echo Building and pushing Docker images
./scripts/docker-publish-ci.sh