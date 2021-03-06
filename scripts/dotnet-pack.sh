#!/bin/bash
cd src/Collectively.Services.Storage.Models
dotnet pack /p:PackageVersion=2.0.$TRAVIS_BUILD_NUMBER --no-restore -o .

echo Uploading Collectively.Services.Storage.Models package to MyGet using branch $TRAVIS_BRANCH

case "$TRAVIS_BRANCH" in
  "master")
    dotnet nuget push *.nupkg -k $MYGET_API_KEY -s https://www.myget.org/F/collectively/api/v2/package
    ;;
  "develop")
    dotnet nuget push *.nupkg -k $MYGET_DEV_API_KEY -s https://www.myget.org/F/collectively-dev/api/v2/package
    ;;    
esac