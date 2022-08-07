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

## Attributes
---
### **RenderUI**
**Parameter:** Class (RenderClass)</br>
**Usage:** Use on classes and methods. Set the *Class* Parameter for classes to define their type</br>
**Effect:** Renders the service-class in the selection list and makes the selected methods able to get choosen.</br>
**Example:** </br>
On Class: `[RenderUI(Class = RenderClass.Cipher)]` </br>
On Method: `[RenderUI]`

### **UIParam**
**Parameter:** 
- Name (String)
- DefaultValue (String)

**Usage:** Use in front of parameter to specify meta information</br>
**Effect:** Gives the control label the defined *Name* and adds the *DefaultValue* as initial control value</br>
**Example:** `([UIParam("Key Size", "1024")] Int32 keySize)`

### **UseCopy**
**Parameter:** CopyFunctions (List<\String>) <br/>
**Usage:** Use on method where the output is able to be copied by defined copy function</br>
**Effect:** Renders button for defined copy button on output textarea<br/>
**Example:** `[UseCopy("CopyPublicKey", "CopyPrivateKey")]`

### **CopyRoutine**
**Parameter:** Name (String) <br/>
**Usage:** Use on copy function</br>
**Effect:** Makes the copy function available on the ui<br/>
**Example:** `[CopyRoutine("Copy Public Key")]`

> To look up example services you can access the [library](https://github.com/LukasStolz314/ncrypt.Library) project, where you can find sample services
