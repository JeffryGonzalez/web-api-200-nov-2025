# The VIP API

It needs to support a client:

## HelpDesk.Api

We check for VIPs by sending a request, and getting a response for each new issue created.

Example:

```http
POST /vip-check
Content-Type: application/json

{
    "userSub": "bob@company.com"
}
```

It expects a response like:

```http
200 Ok
Content-Type: application/json

{
    "userSub": "bob@company.com",
    "isVip": false
}
```

[See Test For Example](../HelpDesk.Tests/Vips/UsingTheClient.cs)

## Help Desk Managers (This will be the lab)

Want to be able to assign user accounts as VIPs.

They also need to "deactivate" the VIP status.

They want a way to get a list of all VIPs, both active and inactive.

- Go To http://localhost:5038/scalar/


## Adding VIPS

```http
POST http://localhost:5038/management/vips HTTP/1.1
Host: localhost
Content-Type: application/json

{"userSubject":"","reason":""}
```

Should return:

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "userSubject": "string",
  "reason": "string",
  "created": "2025-11-13T18:27:58.261Z"
}
```

## Getting VIPs

```http
GET http://localhost:5038/management/management/vips HTTP/1.1
Host: localhost
```

Should Return

```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userSubject": "string",
    "reason": "string",
    "created": "2025-11-13T18:27:58.261Z"
  }
]
```

See the rest of the docs for examples.


After you are done with this, make the /vip-check endpoint "real"

