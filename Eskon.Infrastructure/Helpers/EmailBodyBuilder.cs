using System.Reflection;
using System.Text;

namespace Eskon.Infrastructure.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string templateName, Dictionary<string, string> templateModel)
    {
        // var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        // var streamReader = new StreamReader(templatePath);
        // var body = streamReader.ReadToEnd();
        // streamReader.Close();
        //
        // foreach (var item in templateModel)
        //     body = body.Replace(item.Key, item.Value);
        //
        // return body;
        
        var template = GetTemplate(templateName);

        var emailBody = new StringBuilder(template);

        foreach (var item in templateModel)
            emailBody.Replace(item.Key, item.Value);

        return emailBody.ToString();
    }
    
    private static string GetTemplate(string templateName)
    {
        var resourceName = $"Eskon.Infrastructure.Templates.{templateName}.html";
            
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new FileNotFoundException(
                $"The email template '{resourceName}' was not found as an embedded resource in the assembly '{assembly.FullName}'. " +
                "Ensure the file exists and its 'Build Action' property is set to 'Embedded resource'.", 
                resourceName);
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

}