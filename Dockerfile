# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app


COPY ./src ./

RUN dotnet restore

RUN dotnet publish -c Release -o /app/out   

# Step 2 Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app/out  .


# Set the entry point to the published app
ENTRYPOINT ["dotnet", "ContainerizedAPI.dll"]
