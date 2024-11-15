# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5089

# Use the official build image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and restore dependencies
COPY UpDownCryptorollBackend.sln ./
COPY Bll/ Bll/
COPY Dal/ Dal/
COPY Pl/ Pl/

RUN dotnet restore Pl/Pl.csproj

# Copy the entire source and build the app
COPY . .
WORKDIR /src/Pl
RUN dotnet publish Pl.csproj -c Release -o /app/publish

# Final stage: copy the build output
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Pl.dll"]
