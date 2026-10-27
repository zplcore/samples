#!/usr/bin/env bash
# zplCore Server on Azure Container Apps (Azure CLI with the containerapp extension).
# Label templates come from Azure Blob Storage, read with the app's managed identity.
# The API key is read from your shell (ZPLCORE_API_KEY) into a Container Apps secret, never into a file.
set -euo pipefail

RESOURCE_GROUP=zplcore-rg
LOCATION=westeurope
ENVIRONMENT=zplcore-env
APP=zplcore-server
STORAGE_ACCOUNT="<your-storage-account>"   # holds the container "labels" with the template files
: "${ZPLCORE_API_KEY:?export ZPLCORE_API_KEY (at least 16 characters) first}"

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"
az containerapp env create --name "$ENVIRONMENT" --resource-group "$RESOURCE_GROUP" --location "$LOCATION"

az containerapp create \
  --name "$APP" --resource-group "$RESOURCE_GROUP" --environment "$ENVIRONMENT" \
  --image docker.io/zplcore/server:1.0.0 \
  --target-port 8080 --ingress external \
  --cpu 1 --memory 2Gi --min-replicas 1 --max-replicas 5 \
  --system-assigned \
  --secrets "api-key=$ZPLCORE_API_KEY" \
  --env-vars \
    ZPLCORE__AUTH__MODE=apikey \
    ZPLCORE__AUTH__APIKEYS__0=secretref:api-key \
    ZPLCORE__TEMPLATES__SOURCES__1__NAME=azure \
    ZPLCORE__TEMPLATES__SOURCES__1__TYPE=azureblob \
    ZPLCORE__TEMPLATES__SOURCES__1__CONTAINER=labels \
    ZPLCORE__TEMPLATES__SOURCES__1__SERVICEURL="https://$STORAGE_ACCOUNT.blob.core.windows.net" \
    ZPLCORE__TEMPLATES__SOURCES__1__IDENTITY=managed

# let the app's identity read the templates
PRINCIPAL_ID=$(az containerapp show --name "$APP" --resource-group "$RESOURCE_GROUP" --query identity.principalId -o tsv)
STORAGE_ID=$(az storage account show --name "$STORAGE_ACCOUNT" --query id -o tsv)
az role assignment create --assignee "$PRINCIPAL_ID" --role "Storage Blob Data Reader" --scope "$STORAGE_ID"

az containerapp show --name "$APP" --resource-group "$RESOURCE_GROUP" --query properties.configuration.ingress.fqdn -o tsv
