# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
ARG JWT_KEY
ARG JWT_REFRESH_KEY
ARG DEFAULT_CONNECTION
ARG HOST_URL
ARG HOST_API
ARG TWILIO_SID
ARG TWILIO_TOKEN
ARG TWILIO_VERIFY_SID
ARG BREVO_API_KEY
ARG BREVO_API_URL
ARG BREVO_FROM_EMAIL
ARG CLOUDINARY_CLOUD_NAME
ARG CLOUDINARY_API_KEY
ARG CLOUDINARY_API_SECRET

ENV JWT_KEY=${JWT_KEY}
ENV JWT_REFRESH_KEY=${JWT_REFRESH_KEY}
ENV DEFAULT_CONNECTION=${DEFAULT_CONNECTION}
ENV HOST_URL=${HOST_URL}
ENV HOST_API=${HOST_API}
ENV TWILIO_SID=${TWILIO_SID}
ENV TWILIO_TOKEN=${TWILIO_TOKEN}
ENV TWILIO_VERIFY_SID=${TWILIO_VERIFY_SID}
ENV BREVO_API_KEY=${BREVO_API_KEY}
ENV BREVO_API_URL=${BREVO_API_URL}
ENV BREVO_FROM_EMAIL=${BREVO_FROM_EMAIL}
ENV CLOUDINARY_CLOUD_NAME=${CLOUDINARY_CLOUD_NAME}
ENV CLOUDINARY_API_KEY=${CLOUDINARY_API_KEY}
ENV CLOUDINARY_API_SECRET=${CLOUDINARY_API_SECRET}

COPY ["TiffinMate.API/TiffinMate.API.csproj", "TiffinMate.API/"]
COPY ["TiffinMate.BLL/TiffinMate.BLL.csproj", "TiffinMate.BLL/"]
COPY ["TiffinMate.DAL/TiffinMate.DAL.csproj", "TiffinMate.DAL/"]
RUN dotnet restore "./TiffinMate.API/TiffinMate.API.csproj"
COPY . .
WORKDIR "/src/TiffinMate.API"
RUN dotnet build "./TiffinMate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TiffinMate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TiffinMate.API.dll"]