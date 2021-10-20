FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
ENV PATH="root/.dotnet/tools:${PATH}"
RUN dotnet tool install --global Cake.Tool

FROM builder AS release
WORKDIR /app
COPY . /app/
RUN dotnet tool restore
RUN dotnet cake --pack
