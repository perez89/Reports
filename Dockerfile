FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
RUN apt-get update && apt-get -qy install netcat && wget https://raw.githubusercontent.com/eficode/wait-for/master/wait-for -O /bin/wait-for.sh && chmod +x /bin/wait-for.sh

WORKDIR /src

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
COPY /src/ .

FROM build-env as test
RUN dotnet build *.sln

FROM build-env AS publish

# Build and publish a release
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.dll"]