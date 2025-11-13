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

