#!/usr/bin/env sh

docker build -f Docker/Dockerfile -t metaprint-build . && \
docker create --name art metaprint-build && \
docker cp art:/app/artifacts ./artifacts && \
docker rm art