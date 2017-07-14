#!/bin/bash
DOCKER_ENV=''
DOCKER_TAG=''
case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
  "develop")
    DOCKER_ENV=development
    DOCKER_TAG=dev
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -f ./src/Collectively.Services.Storage/Dockerfile.$DOCKER_ENV -t collectively.services.storage:$DOCKER_TAG ./src/Collectively.Services.Storage
docker tag collectively.services.storage:$DOCKER_TAG $DOCKER_USERNAME/collectively.services.storage:$DOCKER_TAG
docker push $DOCKER_USERNAME/collectively.services.storage:$DOCKER_TAG