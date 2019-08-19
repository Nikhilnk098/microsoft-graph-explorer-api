---
title: "Java examples"
description: "Examples Snippets generated by the api"
author: "andrueastman"
---

# Java examples

This shows how snippets requests look like for Java

## Examples

### GET Request

#### Example GET Request

```http
GET /v1.0/me/events/AAMkAGIAAAoZDOFAAA=?$select=subject,body,bodyPreview,organizer,attendees,start,end,location HTTP/1.1
Host: graph.microsoft.com
Prefer: outlook.timezone="Pacific Standard Time"

```

#### Example GET Request snippet generated

```java
IGraphServiceClient graphClient = GraphServiceClient.builder().authenticationProvider( authProvider ).buildClient();

LinkedList<Option> requestOptions = new LinkedList<Option>();
requestOptions.add(new HeaderOption("Prefer", "outlook.timezone=\"Pacific Standard Time\""));

Event event = graphClient.me().events("AAMkAGIAAAoZDOFAAA=")
	.buildRequest( requestOptions )
	.select("subject,body,bodyPreview,organizer,attendees,start,end,location")
	.get();
```

### POST Request

#### Example POST Request

```http
POST /v1.0/users HTTP/1.1
Host: graph.microsoft.com
Content-type: application/json

{
  "accountEnabled": true,
  "displayName": "displayName-value",
  "mailNickname": "mailNickname-value",
  "userPrincipalName": "upn-value@tenant-value.onmicrosoft.com",
  "passwordProfile" : {
    "forceChangePasswordNextSignIn": true,
    "password": "password-value"
  }
}
```

#### Example POST Request snippet generated

```java
IGraphServiceClient graphClient = GraphServiceClient.builder().authenticationProvider( authProvider ).buildClient();

User user = new User();
user.accountEnabled = true;
user.displayName = "displayName-value";
user.mailNickname = "mailNickname-value";
user.userPrincipalName = "upn-value@tenant-value.onmicrosoft.com";
PasswordProfile passwordProfile = new PasswordProfile();
passwordProfile.forceChangePasswordNextSignIn = true;
passwordProfile.password = "password-value";
user.passwordProfile = passwordProfile;

graphClient.users()
	.buildRequest()
	.post(user);
```

### PATCH Request

#### Example PATCH Request

```http
PATCH /v1.0/me HTTP/1.1
Host: graph.microsoft.com
Content-type: application/json
Content-length: 491

{
  "accountEnabled": true,
  "businessPhones": [
    "businessPhones-value"
  ],
  "city": "city-value"
}
```

#### Example PATCH Request snippet generated

```java
IGraphServiceClient graphClient = GraphServiceClient.builder().authenticationProvider( authProvider ).buildClient();

User user = new User();
user.accountEnabled = true;
LinkedList<String> businessPhonesList = new LinkedList<String>();
businessPhonesList.add("businessPhones-value");
user.businessPhones = businessPhonesList;
user.city = "city-value";

graphClient.me()
	.buildRequest()
	.patch(user);
```

### PUT Request

#### Example PUT Request

```http
PUT /beta/applications/{id}/synchronization/templates/{templateId} HTTP/1.1
Host: graph.microsoft.com
Authorization: Bearer <token>
Content-type: application/json

{
    "id": "Slack",
    "applicationId": "{id}",
    "factoryTag": "CustomSCIM"
}
```

#### Example PUT Request snippet generated

```java
IGraphServiceClient graphClient = GraphServiceClient.builder().authenticationProvider( authProvider ).buildClient();

LinkedList<Option> requestOptions = new LinkedList<Option>();
requestOptions.add(new HeaderOption("Authorization", "Bearer <token>"));

SynchronizationTemplate synchronizationTemplate = new SynchronizationTemplate();
synchronizationTemplate.id = "Slack";
synchronizationTemplate.applicationId = "{id}";
synchronizationTemplate.factoryTag = "CustomSCIM";

graphClient.applications("{id}").synchronization().templates("{templateId}")
	.buildRequest( requestOptions )
	.put(synchronizationTemplate);
```

### DELETE Request

#### Example DELETE Request

```http
DELETE /v1.0/me/messages/{id} HTTP/1.1
Host: graph.microsoft.com

```

#### Example DELETE Request snippet generated

```java
IGraphServiceClient graphClient = GraphServiceClient.builder().authenticationProvider( authProvider ).buildClient();

graphClient.me().messages("{id}")
	.buildRequest()
	.delete();
```