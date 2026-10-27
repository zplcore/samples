# Azure Container Apps

`deploy.sh` creates a Container Apps environment and runs zplCore Server with an external HTTPS endpoint. The API key
is stored as a Container Apps secret, the label templates come from Azure Blob Storage and are read with the app's
managed identity (no storage keys in the container).

## Prerequisites

- Azure CLI with the `containerapp` extension (`az extension add --name containerapp`), logged in with `az login`
- a storage account with a blob container `labels` that holds your templates (e.g. `shipping/label-100x150.json`)

## Run

```sh
cd docker/azure-container-apps
# set STORAGE_ACCOUNT (and the names) at the top of deploy.sh, then:
export ZPLCORE_API_KEY=...      # at least 16 characters
./deploy.sh                     # prints the host name of the app

curl -X POST https://<app-host>/v1/labels/<template> -H "X-Api-Key: $ZPLCORE_API_KEY" \
  -H 'Content-Type: application/json' -d '{"name":"Hello zplCore"}'
```

The role assignment can take a few minutes; until then `/ready` reports the template source as not readable.

- Licence key: add a second secret (`--secrets license=...`) and `ZPLCORE__LICENSE__KEY=secretref:license`.
  Without it the server runs in evaluation mode with a watermark.
- Instead of a secret value you can reference Azure Key Vault (`keyvaultref:` secrets with a managed identity).
- For single sign-on with Entra ID use mode `jwt`: [authentication](https://zplcore.com/docs/server/#auth).
