Remember our "axis" of design in HTTP are:
    - Resources - names of important thingies.
    - Methods - GET, POST, PUT, DELETE, HEAD, etc.
    - Representations - Stuff we send to the server or retrieve from the server.
    
- Also, HTTP is "synchronous" - Request/Response.

REsources are identified through a URI.
    - they are usually one of the followings "kinds" of resources:
        - Collections (lists, sets of things) e.g. `/employees`
        - Documents (specific instances of something) `/employees/bob-smith`
            - bob smith could have "issues" `/employees/bob-smith/issues`
        - Store (mirror of client state on the server)
            - usually used for things like what we are doing here
            - an example would be `GET https://amazon.com/shoppingcart`
                - this would require an Authorization header.
                - it *could* be an alias for another resource:
                    - e.g. `https://amazon.com/users/{userId}/shoppingcart`

        - Controller (weird name, but these are rare)


# Request

What do we need to know:
    - Who is the employee making the request?
        - The WHO in APIs is ALWAYS ALWAYS ALWAYS the authorization header
        - a *reference* - the JWT of the user as issued from the IDP
    - What software are they having a problem with?
        - has to be an approved piece of software from the software team.
        - what if it isn't? Do we just not let them file an issue?
    -   - a *reference* to some data owned by another team (the software team)
    - description: tell us what your problem is.
    - impact: "Inconvenience" | "WorkStoppage"
    - impactRadius: "Personal" | "Customer"


```http
POST http://localhost:1338/employee/issues
Content-Type: application/json

{
    "softwareId": "5141619f-4b9a-4053-8289-ec4bb6cc5d02",
    "description": "Arms week event is too hard. Riderxxxxx",
    "impact": "WorkStoppage",
    "impactRadius": "Customer",
    "contactMechanisms": {
        "email": "jeff@company.com",
        "phone": "555-1212"
    },
    "contactPreference": "Email"
}

```

```http
GET http://localhost:1338/demos/software/f81dbfab-2a30-4e76-98e4-d1a67799711e
```


```http
GET http://localhost:1338/employee/issues/2e19bd5e-c9d3-4ee8-965f-6af49b27152c
```

```http
GET http://localhost:1338/employee/issues
```

```http
GET http://localhost:1338/issues-awaiting-tech-assignment
```
## Response

```http
201 Created
Location: http://localhost:1338/employee/issues/{issueId}
Content-Type: application/json 

{
    "id": "{{issueId}}"
    "softwareId": "some-id-from-the-software-team",
    "description": "long form description of the issue",
    "impact": "WorkStoppage",
    "impactRadius": "Customer",
    "contactMechanisms": {
        "email": "jeff@company.com",
        "phone": "555-1212"
    },
    "contactPreference": "Email",
    "submittedBy": "name in token",
    "submittedAt": "date time offset of when it was submitted",
    "status": "AwaitingVerification" | "AwaitingTechAssignment" | "ElevatedToVipManager" | ... 

}
```


```http
GET http://localhost:1338/debug/routes
```

```http
GET http://localhost:1337/open-issues
```

```http
GET http://localhost:1337/employees/{id}/issues
Authorization: Bearer (some token, maybe for a manager or something)
```
# Response

GET http://localhost:1337/employee/issues
GET http://localhost:1337/employees/9839389389/issues

## Message Stuff

```json
{
  "id": "2e19bd5e-c9d3-4ee8-965f-6af49b27152c",
  "version": 3,
  "description": "Arms week event is too hard. Riderxxxxx",
  "impact": "WorkStoppage",
  "impactRadius": "Customer",
  "software": {
    "id": "5141619f-4b9a-4053-8289-ec4bb6cc5d02",
    "retrievedAt": "2025-11-12T20:16:52+00:00",
    "retrievedFrom": "http://software-center.company.com/help-desk/catalog-items/5141619f-4b9a-4053-8289-ec4bb6cc5d02",
    "title": "Visual Studio Code",
    "vendor": "Microsoft"
  },
  "vipStatus": "Is Vip",
  "softwareChecked": true,
  "vipStatusChecked": true,
  "assignedPriority": 600,
  "submittedBy": "9be7c56a-45cb-43df-960a-4eed2c7ce147",
  "contactMechanisms": {
    "email": "jeff@company.com",
    "phone": "555-1212"
  },
  "contactPreferences": "Email",
  "status": "AwaitingTechAssignment"
}
```

Software is a "reference" to some data owned by someone else and this is a copy of it at a point in time.

Scenario:

User submits an issue on tuesday for a piece of software that is supported.
Tuesday night, the software center retires it.
Wednesday, a tech picks this up, and can see there was some software, but maybe finds out it isn't supported anymore. 


## Example 

```http
POST /store/orders
Authorization: Bearer (the id of the customer)
Content-Type: application/json

{
    "item": {
        "sku": 3282,
        "retrievedAt": "{some date time}",
        "name": "Beer",
        "price": 12.99,
        "qty": 1
    },
    "total": 12.99
}
````

You are the orders api


There is another api that has the products -
that embedded product is from them.

## Alternative Aggreagation of Events


```json
{
    "id": "issue-id",
    "history": [
        { "when": "TimeItHappenend", "event": "IssueCreated"},
        { "when": "TimeItHappened", "event", "VipIssue"}
    ]
}


```

```http
GET http://localhost:1338/issue-history/d06ba34a-d41f-4dc5-a9ab-ce4f40038c1f?version=2
```