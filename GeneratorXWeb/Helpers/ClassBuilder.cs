using System.Text;

public class ClassBuilder
{
    private StringBuilder _builder;
    private int _indentLevel;

    public ClassBuilder()
    {
        _builder = new StringBuilder();
        _indentLevel = 0;
    }

    private void AppendIndentedLine(string line)
    {
        _builder.AppendLine(new string('\t', _indentLevel) + line);
    }

    public ClassBuilder AddUsingDirective(string namespaceName)
    {
        _builder.AppendLine($"using {namespaceName};");
        return this;
    }

    public ClassBuilder StartNamespace(string namespaceName)
    {
        AppendIndentedLine($"namespace {namespaceName}");
        AppendIndentedLine("{");
        _indentLevel++;
        return this;
    }

    public ClassBuilder EndAction()
    {
        _indentLevel--;
        AppendIndentedLine("}");
        return this;
    }

    public ClassBuilder StartClassDeclaration(string className, string baseClassName = null, bool isPartial = false)
    {
        AppendIndentedLine($"public {(isPartial ? "partial " : "")}class {className} : {baseClassName ?? "object"}");
        AppendIndentedLine("{");
        _indentLevel++;
        return this;
    }



    public ClassBuilder AddPrivateReadonlyField(string typeName, string fieldName)
    {
        AppendIndentedLine($"private readonly {typeName} {fieldName};");
        return this;
    }

    public ClassBuilder AddProperty(string typeName, string propertyName)
    {
        AppendIndentedLine($"public {typeName} {propertyName} {{ get; set; }}");
        return this;
    }

    public ClassBuilder StartConstructor(string constructorName, string parameters)
    {
        AppendIndentedLine($"public {constructorName}({parameters})");
        AppendIndentedLine("{");
        _indentLevel++;
        return this;
    }

    public ClassBuilder AssignFieldFromParameter(string parameterName)
    {
        AppendIndentedLine($"this.{parameterName} = {parameterName};");
        return this;
    }



    public ClassBuilder AddMethod(string returnType, string methodName, string body, string parameterList = "", bool isPrivate = false)
    {
        AppendIndentedLine($"{(isPrivate ? "private" : "public")} {returnType} {methodName}({parameterList})");
        AppendIndentedLine("{");
        _indentLevel++;
        AppendIndentedLine(body);
        _indentLevel--;
        AppendIndentedLine("}");
        return this;
    }



    public ClassBuilder AddAttribute(string attributeName, string attributeParameters = "")
    {
        AppendIndentedLine($"[{attributeName}({attributeParameters})]");
        return this;
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}
