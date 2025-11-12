# Welcome Back

- https://class.hypertheory-labs.com/guacamole
- userid here is "studentX" - where X is your student number
- password here is "Hypertheory_Training!"
- On the black and white login screen:
    - user is "student" (no number)
    - password is whatever you changed your VM password to yesterday.

- When you are in your VM, please start Docker Desktop

## Today

- Moving to Docker Compose Fix
  - The Problem seems to be mostly the VMs - nested virtualization
- Event Sourcing
  - Building up states from a series of events over time.
  - "We want to show what the issue is after all these things have happened"
  - Projections
    - Inline Projections - Create the Read Model based on demand - it is not stored.
    - Live Projections - Every time an event happens the read model is updated immediately (as part of the same projection)
    - Async Projections - Runs in a separate process or even other "nodes". Eventual Consistency. More Scalable.
- Code Changes
  - Cleaned Up The Handlers
  - Created a SingleStream Projection
  - 