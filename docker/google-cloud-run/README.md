# Google Cloud Run

`service.yaml` runs zplCore Server on Cloud Run: the API key comes from Secret Manager, the label templates from a
Cloud Storage bucket that the service account may read (no key file in the container).

## Prerequisites

- Google Cloud CLI (`gcloud`), a project with the Cloud Run and Secret Manager APIs enabled
- a bucket with your templates below `templates/` (e.g. `templates/shipping/label-100x150.json`)

## Run

```sh
cd docker/google-cloud-run
PROJECT=<project-id>; REGION=europe-west1; BUCKET=<your-template-bucket>

# service account with read access to the templates and the secret
gcloud iam service-accounts create zplcore-server
gcloud storage buckets add-iam-policy-binding gs://$BUCKET \
  --member serviceAccount:zplcore-server@$PROJECT.iam.gserviceaccount.com --role roles/storage.objectViewer

# API key as a secret (the value never goes into a file in the repository)
printf '%s' "$ZPLCORE_API_KEY" | gcloud secrets create zplcore-api-key --data-file=-
gcloud secrets add-iam-policy-binding zplcore-api-key \
  --member serviceAccount:zplcore-server@$PROJECT.iam.gserviceaccount.com --role roles/secretmanager.secretAccessor

# replace <project-id> and <your-template-bucket> in service.yaml, then:
gcloud run services replace service.yaml --region $REGION
gcloud run services add-iam-policy-binding zplcore-server --region $REGION \
  --member allUsers --role roles/run.invoker      # the server checks the API key itself

curl -X POST https://<service-url>/v1/labels/<template> -H "X-Api-Key: $ZPLCORE_API_KEY" \
  -H 'Content-Type: application/json' -d '{"name":"Hello zplCore"}'
```

Licence key: store it as a second secret and add `ZPLCORE__LICENSE__KEY` with a `secretKeyRef` to `service.yaml`.
Without it the server runs in evaluation mode with a watermark. Settings: [server documentation](https://zplcore.com/docs/server/).
