# `cloud-run-asp-net-http2`

Example using Cloud Run and a ASP.NET service streaming data back using HTTP2.

```bash
dotnet --version
8.0.100-rc.2.23502.2
```

Test locally:
```bash
dotnet run
curl -v --http2-prior-knowledge http://localhost:5000/numbers
```

After creating a project or choosing an existing one, make sure it is used by default (the environment-variable will be used by further commands):
```bash
export PROJECT=cloud-run-asp-net-http2
gcloud config set project $PROJECT
```

If organization policies prohibit a public service, you can override them:
```bash
gcloud services enable orgpolicy.googleapis.com
jq -n --arg project $PROJECT '{"name":"projects/\($project)/policies/iam.allowedPolicyMemberDomains","spec":{"rules": [{"allowAll":true}]}}' > my-policy.json ; gcloud org-policies set-policy my-policy.json ; rm my-policy.json
```

Enable the necessary Google Cloud services:
```bash
gcloud services enable compute.googleapis.com
gcloud services enable sourcerepo.googleapis.com
gcloud services enable run.googleapis.com
gcloud services enable cloudbuild.googleapis.com
```

Deploy directly from source:
```bash
gcloud run deploy asp-net-http2 --use-http2 --port=8080 --allow-unauthenticated --region=europe-west1 --source=.
```

Example output (from above):
```
Deploying from source requires an Artifact Registry Docker repository to store built containers. A repository named [cloud-run-source-deploy] in region
[europe-west1] will be created.

Do you want to continue (Y/n)? Y

This command is equivalent to running `gcloud builds submit --tag [IMAGE] .` and `gcloud run deploy asp-net-http2 --image [IMAGE]`

Building using Dockerfile and deploying container to Cloud Run service [asp-net-http2] in project [cloud-run-asp-net-http2] region [europe-west1]
Building and deploying new service...
  Creating Container Repository...done
  Uploading sources...done
  Building Container... Logs are available at [https://console.cloud.google.com/cloud-build/builds/6b42cc43-e340-4c23-82e2-e6a6fa627b63?project=795402527316
  ]....done
  Setting IAM Policy...done
  Creating Revision...done
  Routing traffic...done
Done.
Service [asp-net-http2] revision [asp-net-http2-00001-z46] has been deployed and is serving 100 percent of traffic.
Service URL: https://asp-net-http2-2xhili4xbq-ew.a.run.app
```

Test the Cloud Run service:
```
curl -v https://asp-net-http2-2xhili4xbq-ew.a.run.app/numbers
```

