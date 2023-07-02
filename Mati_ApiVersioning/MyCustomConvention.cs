using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;

public class MyCustomConvention : IControllerConvention
{
    public bool Apply(IControllerConventionBuilder controller, ControllerModel controllerModel)
    {
        var isHelloWorld = controllerModel.ControllerName == "HelloWorld";
        if (isHelloWorld)
        {
            controller.IsApiVersionNeutral();
            return false;
        }

        return true;
    }
}

public class MyApiControllerFilter : IApiControllerFilter
{
    public IList<ControllerModel> Apply(IList<ControllerModel> controllers)
    {
        var result =
            controllers.Where(_ => _.ControllerName != "HelloWorld")
            .ToList()
        ;
        return result;
    }
}

public class NonUIControllerSpecification : IApiControllerSpecification
{
    private readonly Type UIControllerType = typeof(Controller).GetTypeInfo();

    public bool IsSatisfiedBy(ControllerModel controller) =>
        !UIControllerType.IsAssignableFrom(controller.ControllerType);
}