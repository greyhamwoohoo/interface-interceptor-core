# interface-interceptor-core
Intercept calls to one or more interface methods and (optionally) provide OnBefore, OnAfter and Stub callbacks.

## What is it good for?
Intercept interface calls made on a concrete implementation: Capture and replay return values / parameters at the 'edge' of your (in-process) API, service or app. 

Ideal for testing (particularly: in-process component testing). Using this package implies your test framework has the ability to override the dependency injection container before service initialization and can be executed in two workflows: Capture; and Replay. 

Optionally provide callbacks OnBefore, OnAfter or Stub the entire execution of the interface method. 

For example: if you have an Interface called ITheInterface with a method called DoSomething() that returns an int value, the following will provide a callback to capture the return value after the concrete implementation has been invoked:

```csharp
private readonly TheInterfaceImplementation _originalImplementation = new TheInterfaceImplementation();

var theReturnValue = default(int);

var _interceptedInterface = new InterceptorProxyBuilder<ITheInterface>()
    .For(_originalImplementation)
    .InterceptAfterExecutionOf(theMethodCalled: nameof(ITheInterface.DoSomething), andCallbackWith: result =>
    {
        theReturnValue = (int) result.ReturnValue;
    })
    .Build();;

// Invoke the interface: the original implementation will be called and then the above handler invoked. 
var result = _interceptedInterface.DoSomething();
 ```
 See the .UnitTests project for how to configure OnBefore, OnAfter and Stub Executions in place of the original method. 

## Limitations
Many! This is not intended to be a unit testing or universal mocking tool - more an intellectual exercise so I could learn DispatchProxy :)

Main limitations are:

1. Works with methods only (no properties at present)
2. Method overloads are not supported
3. Probably lots, lots more I haven't considered. 

## Can't you do this with Unit Testing / Mocking tools?
Yeah! Almost certainly in most cases. Try it :)

## References
| Reference | Link |
| --------- | ---- |
| Aspect Oriented Programming example (via DispatchProxy) | https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/ |
| Share variables across Azure DevOps Pipelines | https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops&tabs=yaml%2Cbatch#share-variables-across-pipelines |
| Perfecting CD for NuGET Packages | https://cloudblogs.microsoft.com/industry-blog/en-gb/technetuk/2019/06/18/perfecting-continuous-delivery-of-nuget-packages-for-azure-artifacts/ |
| Overriding NuGET Properties (FileVersion etc) | https://docs.microsoft.com/en-us/dotnet/core/tools/csproj |
| Custom counters for Azure DevOps / Semantic Versioning | https://kasunkodagoda.com/2019/04/03/hidden-gems-in-azure-pipelines-creating-your-own-rev-variable-using-counter-expression-in-azure-pipelines/ |
