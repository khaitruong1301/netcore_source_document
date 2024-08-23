using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CustomHeaderAttribute : Attribute
{
    public string HeaderName { get; }
    public string Description { get; }

    public CustomHeaderAttribute(string headerName, string description)
    {
        HeaderName = headerName;
        Description = description;
    }
}
