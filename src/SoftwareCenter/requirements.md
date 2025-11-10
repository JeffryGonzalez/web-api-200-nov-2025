# Software Center 

They maintain the list of supported software for our company.

We are building them an API.

## Vendors

We have arrangements with vendors. Each vendor has:

- And ID we assign
- A Name
- A Website URL
- A Point of Contact
  - Name
  - Email
  - Phone Number

Vendors have a set of software they provide that we support.


## Catalog Items

Catalog items are instances of software a vendor provides.

A catalog item has:
- An ID we assign
- A vendor the item is associated with
- The name of the software item
- A description
- A version number (we prefer SEMVER, but not all vendors use it)



Missing stuff on the request - like name, description, etc. - 400
Vendor Id: has to be in the "form" of a Guid, and...... it could be for a vendor we don't currently support.



Note - One catalog item may have several versions. Each is it's own item.

## Use Cases

The Software Center needs a way for managers to add vendors. Normal members of the team cannot add vendors.
Software Center team members may add catalog items to a vendor.
Software Center team members may add versions of catalog items.
Software Center may deprecate a catalog items. (effectively retiring them, so they don't show up on the catalog)

Any employee in the company can use our API to get a full list of the software catalog we currently support.

- none of this stuff can be used unless you are verified (intentified) as an employee.
- some employees are:
  - members of the software center team
    - and some of them are managers of that team



## The Catalog Items

### Find a Vendor


### Get a List of Catalog Items For That Vendor 


### Get All Catalog Items

- if that vendor doesn't exist, return a 404
- if the vendor doesn't have any items, return a 200 with an empty set

### Add A Catalog Item

- Must be a member of the software team
