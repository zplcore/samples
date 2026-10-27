# AWS ECS on Fargate

`task-definition.json` runs zplCore Server on Fargate: the API key and the licence key come from AWS Secrets Manager,
the label templates from an S3 bucket that the task role may read (no access keys in the container).

## Prerequisites

- AWS CLI, an ECS cluster, a VPC with subnets and a security group that allows port 8080 (or a load balancer)
- an S3 bucket with your templates below `templates/` (e.g. `templates/shipping/label-100x150.json`)
- IAM roles:
  - `zplcore-execution-role`: `AmazonECSTaskExecutionRolePolicy` plus `secretsmanager:GetSecretValue` on the two secrets
  - `zplcore-task-role`: `s3:ListBucket` and `s3:GetObject` on the template bucket

## Run

```sh
cd docker/aws-ecs-fargate

# secrets (the values never go into a file in the repository)
aws secretsmanager create-secret --name zplcore/api-key --secret-string "$ZPLCORE_API_KEY"
aws secretsmanager create-secret --name zplcore/license --secret-string "$ZPLCORE_LICENSE_KEY"

# replace <account-id>, <region> and <your-template-bucket> in task-definition.json, then:
aws ecs register-task-definition --cli-input-json file://task-definition.json
aws ecs create-service --cluster <cluster> --service-name zplcore-server \
  --task-definition zplcore-server --desired-count 2 --launch-type FARGATE \
  --network-configuration "awsvpcConfiguration={subnets=[<subnet-id>],securityGroups=[<sg-id>],assignPublicIp=ENABLED}"
```

Without a licence key, remove the `ZPLCORE__LICENSE__KEY` entry from `secrets`: the server then runs in evaluation
mode with a watermark. Secrets Manager ARNs carry a random suffix – use the full ARN that `create-secret` returns.

The image has no shell, so use the health check of the load balancer target group (`/health`) instead of a
container health check. In production put the service behind an Application Load Balancer with HTTPS.

Template sources and settings: [server documentation](https://zplcore.com/docs/server/).
