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
    "softwareId": "f81dbfab-2a30-4e76-98e4-d1a67799711e",
    "description": "Another One From Jeff Where It is About Rider",
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
GET http://localhost:1338/employee/issues/58e4daf4-a450-4ce6-bd34-cb4a294002d4
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

