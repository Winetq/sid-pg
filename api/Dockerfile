FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY SmartPartyApi/*.csproj ./
RUN dotnet restore

COPY SmartPartyApi ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ADD docker-entry.sh /app/docker-entry.sh
RUN chmod +x /app/docker-entry.sh

ARG DB_URL_DEFAULT="mongodb:27017"
ARG DB_NAME_DEFAULT="SmartParty"

ENV DB_URL=$DB_URL_DEFAULT
ENV DB_NAME=$DB_NAME_DEFAULT 

ENTRYPOINT ["/app/docker-entry.sh" ]
