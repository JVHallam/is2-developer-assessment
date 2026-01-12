# Data Exporter

# Answers:
## 0. Critiques
A new project should not be allowed to have warnings at build time
```
C:\Git\is2-developer-assessment\DataExporter\Dtos\ReadPolicyDto.cs(6,23): warning CS8618: Non-nullable property 'PolicyNumber' must contain a non-null value when exiting constructor. Consider declaring the pr
operty as nullable. [C:\Git\is2-developer-assessment\DataExporter\DataExporter.csproj]
```

This de-sensitizes developers to actual issues.
Handling these is especially useful as "Non-nullable" issues can save bugs down the line if dealt with ahead of time.

## 1. The **GetPolicy** method of the **PoliciesController** has already been implemented, but both itself and the **ReadPolicyAsync** function it calls from the service have some logic errors. Find and fix the logic errors and suggest any other improvements you would make to those methods, if any.
- Why were there no checks to check for logic errors before merging?
- Unit tests? Services without unit tests are not ideal, especially now bugs need to be fixed.
- Policy Service should have an interface and the controller should depend on that interface, not the implementation.

The following code is redundant, as Single will throw an exception of nothing is found, or more than one is found.
```
if (policy == null)
{
    return null;
}
```
The change would be to use FirstOrDefaultAsync instead as it will not through an exception.
We can also log and make the decision:
- Do we throw an EntityNotFound exception
- Continue gracefully

If we continue gracefully, the controller will need to return a 404 on null, rather than an OK.

```
private ExporterDbContext _dbContext;

// Can be changed to 
private readonly ExporterDbContext _dbContext;
```

The block scoped namespaces are antiquated. File scoped namespaces have become the norm.

In the controller, there's a mismatch between the names.
```
[HttpGet("{policyId}")]
public async Task<IActionResult> GetPolicy(int id)
```

There's manual mapping done within PolicyService. 
```
        var policyDto = new ReadPolicyDto()
        {
            Id = policy.Id,
            PolicyNumber = policy.PolicyNumber,
            Premium = policy.Premium,
            StartDate = policy.StartDate
        };
```
Either:
- Move this to it's own mapping class
- Pull in a framework like automapper and have a mapping profile for it

- If we're adding services - also add the ServiceExtensions for injecting them, rather than clogging up Program.cs

- The endpoints were also returning tasks, not the result of the tasks. 
    - The user just wants the data, not the Task object and it's internal state
```
{
  "result": {
    "id": 1,
    "policyNumber": "HSCX1001",
    "premium": 200,
    "startDate": "2024-04-01T00:00:00"
  },
  "id": 2,
  "exception": null,
  "status": 5,
  "isCanceled": false,
  "isCompleted": true,
  "isCompletedSuccessfully": true,
  "creationOptions": 0,
  "asyncState": null,
  "isFaulted": false
}
```

# 2. Implement the **GetPolicies** endpoint that should return all existing policies.
- A raw "GetPolicies" endpoint returning all is not a good idea
    - Users very rarely want all
    - A query object would be a good idea or a query string
    - Pagination is a 100% must to reduce server load in the event that they do want everything
    - Pagination can also force the user to take smaller chunks and then rate limit them to reduce it further
    
- Automapper is especially useful here as it ties in quite nicely to EF. Especially around Joins which would need to be done at some point in the future.

- The integration tests are a bit bare, but I was pressed for time on this one.

# 3. Implement the **PostPolicies** endpoint. It should create a new policy based on the data of the DTO it receives in the body of the call and return a read DTO, if successful. 
- Validation?
    - Could have a validator
    - Could also have validation methods to prevent nulls

- Fluent Validation policies are nice, but that's down to licenses and the likes

- Around the /// xml comments 
    - I'm not a big fan. 
    - They lead to a number of "This is a bridge" style bits of documentation
    - Also lead to visual clutter and un-needed upkeep
    - Tend to drift from reality

- There is an integration test that asserts the ID is exactly 6. This is risky in the real world, unless you're tearing down the data between each test, not suite run.

- Tests not sharing the same DTO's was a choice. Test code and application code should be allowed to grow and change independently of one another.

- Idempotency not implmented as it wasn't requested. Definitely suggested with regards to policies.

# 4. The **Note** entity has been created, but it's not yet configured in the **ExporterDbContext**. Add the missing configuration, considering there is a one-to-many relationship between the **Policy** and the **Note** entities, and seed the database with a few notes.

- Take the Note Entity
- Add it to the ExporterDbContext
- One to many relationship between Policy and note

Added a Navigation property for PolicyId:
```
public int PolicyId { get; set; }
public virtual Policy Policy { get; set; }
```

Would have prefered to discard the explicit PolicyId field, but that would have required writing a seperate Database Seeding method, 
which felt like gold plating at this point for a solution that is already over-engineered.

5. Implement the **Export** endpoint. The call receives two parameters from the query string, the **startDate** and the **endDate**. The method needs to retrieve all policies that have a start date between those two dates, and all of their notes. The data should then be mapped to the **ExportDto** class and returned.

- Changed endpoint from post to get as we're using a query string, it seems odd to use a post instead
- Pagination should be considered here - Dates may make it safe though if they're required to be narrow enough

## Remarks

- The tasks can be completed in any order.
- Any third party library can be used to implement some of the functionality required.
- To test the API, any tool like cURL or Postman can be used and the scripts should be included in the submission.