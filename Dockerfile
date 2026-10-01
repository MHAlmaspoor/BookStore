# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution
COPY BookStore.slnx ./

# Copy source
COPY src ./src

# Restore only ProductService API
RUN dotnet restore src/Services/ProductService/BookStore.ProductService.Api/BookStore.ProductService.Api.csproj

# Build
RUN dotnet build \
    src/Services/ProductService/BookStore.ProductService.Api/BookStore.ProductService.Api.csproj \
    --configuration Release --no-restore

# Publish
RUN dotnet publish \
    src/Services/ProductService/BookStore.ProductService.Api/BookStore.ProductService.Api.csproj \
    --configuration Release  --no-build  --output /app/publish


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BookStore.ProductService.Api.dll"]
