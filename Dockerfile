# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia arquivos da solution e dos projetos para dentro do container
COPY *.sln ./
COPY MuscleUp.Web/*.csproj ./MuscleUp.Web/
COPY MuscleUp.DataBase/*.csproj ./MuscleUp.DataBase/
COPY MuscleUp.Dominio/*.csproj ./MuscleUp.Dominio/

# Restaura as dependÍncias
RUN dotnet restore MuscleUp.Web/MuscleUp.Web.csproj

# Copia tudo para dentro da imagem
COPY . .

# Publica o projeto web
RUN dotnet publish MuscleUp.Web/MuscleUp.Web.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MuscleUp.Web.dll"]
