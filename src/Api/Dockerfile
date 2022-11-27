FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /App

# Copy everything
COPY . ./

RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && cp $file ${file%.*}/; done
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Api.dll"]