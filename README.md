## Requirements:

- .Net Core 5.0


### Running the solution

This solution contains two* projects:

- `EncryptionService` (This also has a seperate test project)
- `ApiGateway`

The `EncyrptionService` project can be run without the `ApiGateway` project.
The `ApiGateway` project needs the `EncryptionService` to be running in order to forward requests

Included in the `Requests.http` file is a set of http requests that can be run against the `EncryptionService` and the `ApiGateway` using the Visual Studio Code Http Client


### Running the tests

There is a test project `EncryptionService.Test` that contains unit tests and some integration tests.
These can be run using the command `dotnet test` in the top-level folder

## Further Inforation 

The `EncryptionService` uses the AES symetric encryption algorithm that is part of .NET core

The `KeyManager` is an `InMemoryKeyManager` (In production this should be replaced by a key management service such as AWS KMS).
By default all encryptions keys are kept active during a key rotation, however key-draining is supported by providing a max number active keys to the `InMemoryKeyManager`
- In the fullness of time this would be made configurable using the .NET configuration api


The `ApiGateway` project uses the Ocelot library to configure an API Gatway