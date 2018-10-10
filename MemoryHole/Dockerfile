ARG DOTNET_VERSION=2.0
FROM microsoft/dotnet:${DOTNET_VERSION}-sdk as build
ARG DOTNET_VERSION
WORKDIR /src/MemoryHole
COPY MemoryHole.csproj /src/MemoryHole
RUN dotnet restore /p:TargetFramework=netcoreapp${DOTNET_VERSION}
COPY . .
RUN dotnet publish --output /opt/MemoryHole /p:TargetFramework=netcoreapp${DOTNET_VERSION}


FROM microsoft/dotnet:${DOTNET_VERSION}-runtime as runtime
WORKDIR /opt/MemoryHole
COPY --from=build /opt/MemoryHole /opt/MemoryHole
ENTRYPOINT [ "dotnet" ]
CMD ["MemoryHole.dll"]