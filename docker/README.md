# zplCore Server and Designer: Docker and cloud samples

| Sample | What it shows |
|---|---|
| [quickstart](quickstart/) | Server and designer with `docker compose`, without login (local test only) |
| [api-key](api-key/) | Server and designer protected with API keys from a `.env` file |
| [templates-volume](templates-volume/) | A folder of label templates mounted as a volume, reloaded while the server runs |
| [kubernetes](kubernetes/) | Plain Kubernetes manifests: Deployment, Service, ConfigMap, Secrets |
| [helm](helm/) | Minimal sample Helm chart |
| [aws-ecs-fargate](aws-ecs-fargate/) | AWS ECS on Fargate, secrets from Secrets Manager, templates from S3 |
| [azure-container-apps](azure-container-apps/) | Azure Container Apps, managed identity for Blob Storage |
| [google-cloud-run](google-cloud-run/) | Google Cloud Run, Secret Manager, templates from Cloud Storage |

Images on Docker Hub: `zplcore/server:1.0.0` and `zplcore/designer:1.0.0`. Both listen on port 8080, run as a
non-root user and work with a read-only root file system.

**Authentication:** `ZPLCORE__AUTH__MODE: none` is only for a local test. Everywhere else use `apikey` or `jwt`
([authentication](https://zplcore.com/docs/server/#auth)). Keep API keys and the licence key in the secret store of
your platform – never in the repository.

Documentation: [zplCore Server](https://zplcore.com/docs/server/) · [zplCore Designer](https://zplcore.com/docs/designer/)
