# What the Software Center Needs -

Before we deploy this, the software center needs to fulfill it's pact with us.

We need them to implement an endpoint:

"An Open Host Approach"
```
GET http://api-address/catalog-items/{softwareId:guid}
```

```http
GET http://localhost:1337/help-desk/catalog-items/{softwareId:guid}
```


This should return either a 404, or the following:

```json
{
    "title": "Title of the software",
    "vendor": "The name of the vendor for this software"
}
```