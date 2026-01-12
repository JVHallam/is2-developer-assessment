# Data Exporter

The **Data Exporter** app is a small RESTful API implemented in .NET 6. It manages insurance policies and any notes the brokers might have added to the policies. It also provides a way to query and map the data to a format an external system might require for importing.

# Tasks

## 0. Critiques
A new project should not be allowed to have warnings at build time
```
C:\Git\is2-developer-assessment\DataExporter\Dtos\ReadPolicyDto.cs(6,23): warning CS8618: Non-nullable property 'PolicyNumber' must contain a non-null value when exiting constructor. Consider declaring the pr
operty as nullable. [C:\Git\is2-developer-assessment\DataExporter\DataExporter.csproj]
```

This de-sensitizes developers to actual issues.
Handling these is especially useful as "Non-nullable" issues can save bugs down the line if dealt with ahead of time.

1. The **GetPolicy** method of the **PoliciesController** has already been implemented, but both itself and the **ReadPolicyAsync** function it calls from the service have some logic errors. Find and fix the logic errors and suggest any other improvements you would make to those methods, if any.

## 1. Answers
- Why were there no checks to check for logic errors before merging?
- Unit tests? Services without unit tests are not ideal, especially now bugs need to be fixed.

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

2. Implement the **GetPolicies** endpoint that should return all existing policies.
- This is setting up for a bad time

- TODO: Write up details about pagination


3. Implement the **PostPolicies** endpoint. It should create a new policy based on the data of the DTO it receives in the body of the call and return a read DTO, if successful. 
4. The **Note** entity has been created, but it's not yet configured in the **ExporterDbContext**. Add the missing configuration, considering there is a one-to-many relationship between the **Policy** and the **Note** entities, and seed the database with a few notes.
5. Implement the **Export** endpoint. The call receives two parameters from the query string, the **startDate** and the **endDate**. The method needs to retrieve all policies that have a start date between those two dates, and all of their notes. The data should then be mapped to the **ExportDto** class and returned.

## Remarks

- The tasks can be completed in any order.
- Any third party library can be used to implement some of the functionality required.
- To test the API, any tool like cURL or Postman can be used and the scripts should be included in the submission.