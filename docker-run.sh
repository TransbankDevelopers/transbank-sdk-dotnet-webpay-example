#!/usr/bin/env bash
#get name of the current directory
NAME=$(basename "$PWD")
#run the docker
docker run --rm -p 8080:8080 $NAME