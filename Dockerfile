ARG VARIANT=7.0.404-bookworm-slim-amd64
FROM mcr.microsoft.com/dotnet/sdk:${VARIANT} AS build
WORKDIR /source

COPY Api/*.csproj Api/
COPY Application/*.csproj Application/
COPY Domain/*.csproj Domain/
COPY Persistence/*csproj Persistence/
RUN dotnet restore Api/Api.csproj

COPY Api/ Api/
COPY Application/ Application/
COPY Domain/ Domain/
COPY Persistence/ Persistence/

FROM build AS publish
WORKDIR /source/Api
RUN dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0.14-bookworm-slim-amd64
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet"]
CMD ["Api.dll", "--urls", "http://+:5000"]
