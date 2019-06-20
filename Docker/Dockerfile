FROM mcr.microsoft.com/dotnet/core/sdk:2.2.300-bionic

ENV PATH="root/.dotnet/tools:${PATH}"
RUN dotnet tool install -g Cake.Tool --version 0.33.0
RUN  apt-get update && apt-get install -qq -y libcurl3

