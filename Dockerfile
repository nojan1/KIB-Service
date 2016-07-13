FROM microsoft/dotnet:1.0.0-preview2-sdk

RUN mkdir /app
ADD appsettings.production.json /app/
ADD KIB-Service/src/KIB-Service/ /app/

EXPOSE 5000


WORKDIR /app
RUN dotnet restore

ENV ASPNETCORE_ENVIRONMENT "Production"
ENTRYPOINT ["dotnet", "run"]