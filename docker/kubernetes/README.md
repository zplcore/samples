# Kubernetes (plain manifests)

`zplcore-server.yaml` deploys zplCore Server with two replicas: label templates from a ConfigMap, API keys and the
optional licence key from Secrets, non-root, read-only root file system, readiness `/ready` and liveness `/health`.

## Prerequisites

- a Kubernetes cluster and `kubectl`
- a file `api-keys.txt` with one API key per line (at least 16 characters each) – keep it out of git

## Run

```sh
cd docker/kubernetes

# secrets (never in the repository)
kubectl create secret generic zplcore-api-keys --from-file=api-keys=./api-keys.txt
kubectl create secret generic zplcore-license --from-file=zplcore-license=./zplcore.license   # optional

kubectl apply -f zplcore-server.yaml
kubectl rollout status deployment/zplcore-server

# test through a port-forward
kubectl port-forward service/zplcore-server 8080:80
curl -X POST localhost:8080/v1/labels/hello -H "X-Api-Key: <key>" \
  -H 'Content-Type: application/json' -d '{"name":"Hello zplCore"}'
```

Add your own templates to the ConfigMap `label-templates` (one key per file, e.g. `shipping.json`), or use another
template source (S3, Azure Blob Storage, Google Cloud Storage, Git via git-sync):
[server documentation](https://zplcore.com/docs/server/#kubernetes).

The server keeps no state of its own, so replicas need no sticky sessions. For asynchronous jobs across replicas
configure a job sink (see the documentation).
