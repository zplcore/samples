# Helm chart (sample)

`zplcore-server/` is a minimal sample chart for zplCore Server: Deployment, Service and a ConfigMap with the label
templates from `values.yaml`. Copy it into your own repository and extend it as needed.

## Prerequisites

- a Kubernetes cluster, `kubectl` and Helm 3
- a file `api-keys.txt` with one API key per line (at least 16 characters each) – keep it out of git

## Run

```sh
cd docker/helm

kubectl create secret generic zplcore-api-keys --from-file=api-keys=./api-keys.txt
# optional licence key:
kubectl create secret generic zplcore-license --from-file=zplcore-license=./zplcore.license

helm install labels ./zplcore-server
# with the licence key:
helm install labels ./zplcore-server --set license.secret=zplcore-license

kubectl port-forward service/labels-zplcore-server 8080:80
curl -X POST localhost:8080/v1/labels/hello -H "X-Api-Key: <key>" \
  -H 'Content-Type: application/json' -d '{"name":"Hello zplCore"}'
```

## Values

| Value | Default | Meaning |
|---|---|---|
| `image.repository` / `image.tag` | `zplcore/server` / appVersion | server image |
| `replicaCount` | `2` | number of pods |
| `auth.mode` | `apikey` | `apikey` or `jwt` (`none` only for a local test, see [authentication](https://zplcore.com/docs/server/#auth)) |
| `auth.apiKeysSecret` | `zplcore-api-keys` | existing Secret with the key `api-keys` |
| `license.secret` | empty | existing Secret with the key `zplcore-license`; empty = evaluation mode with watermark |
| `templates` | `hello.json` | label templates, one entry per file |
| `extraEnv` | `[]` | further settings as `ZPLCORE__SECTION__KEY` variables |

For `jwt` add `ZPLCORE__AUTH__AUTHORITY` and `ZPLCORE__AUTH__AUDIENCE` through `extraEnv`. All settings:
[server documentation](https://zplcore.com/docs/server/#configuration).
