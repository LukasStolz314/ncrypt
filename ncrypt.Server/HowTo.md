# Upload own service

## Requirements
1. File has to be a dynamic link library (.dll)
2. Project should be created as .NET 6 library
2. Correct reference to library project ([ncrypt.Library](https://github.com/LukasStolz314/ncrypt.Library))
3. Project format as required

</br>

## Library reference
---
Reference has to be two levels above. For example: </br>
`<ProjectReference Include="..\..\ncrypt.Library\ncrypt.Library.csproj" />`

## Format rules
---
1. The name of the class has to end with *Service*: "{ServiceName}Service". For example: *ExampleService* displays as *Example*
2. The RenderUI attribute of the library must be used for all classes and methods to be rendered. For the service-class the Property *Class* can be set additionaly. With that you can define the type of the service. For Example: `[RenderUI (Class = RenderClass.Cipher)]`
3. The method parameter that should be filled by the input textarea, has to be named *input*.
4. Other parameter names will be rendered with their original name. Optional you can add the UIParam attribute and pass in a string. With that the displayed name will be overwritten.

> To look up example services you can access the [library](https://github.com/LukasStolz314/ncrypt.Library) project, where you can find sample services
