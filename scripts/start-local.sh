#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Services.Storage
dotnet run --no-restore --urls "http://*:10000"