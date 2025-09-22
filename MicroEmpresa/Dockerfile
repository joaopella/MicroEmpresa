# ---------- Base (runtime) ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ---------- Build/Publish ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia só o .csproj para aproveitar cache de restore
COPY MicroEmpresa.csproj ./
RUN dotnet restore MicroEmpresa.csproj

# Copia o restante do código
COPY . .

# Publica (gera binários otimizados para runtime)
RUN dotnet publish MicroEmpresa.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------- Final ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MicroEmpresa.dll"]
