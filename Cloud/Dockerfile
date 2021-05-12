# Define base image
FROM mcr.microsoft.com/dotnet/sdk:5.0.201-alpine3.12 AS builder

# Copy project files
WORKDIR /source
COPY ./CloudPhotoStorageApp.sln .
COPY ./WebApp/WebApp.csproj ./WebApp/

# Restore
RUN dotnet restore 

# Copy all source code
COPY ./WebApp/ ./WebApp/

# Publish
WORKDIR /source
RUN dotnet publish -c Release -o /publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-alpine3.12
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
WORKDIR /app
COPY --from=builder /publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]
