using ncrypt.Library;
using System.Reflection;

namespace ncrypt.Server.Model;

public record ExecuteSequenceModel (String Input, ConvertType InputConvertType, List<GenericModel> sequence, ConvertType OutputConvertType);
public record FormatInputModel (String Input, ConvertType InputConvertType);
public record FormatMethodParametersModel (MethodInfo Method, String Input, GenericModel Generic);
public record FormatConstructorParametersModel (Type Service, GenericModel Generic);