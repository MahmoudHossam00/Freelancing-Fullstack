using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freelancing.Helpers
{
    public class ProposalRangeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            if (value == ValueProviderResult.None)
                return Task.CompletedTask;

            var ranges = new List<(int?, int?)>();
            // Implement parsing logic for query string format
            // Example: "1-3,5-10,null-7"

            bindingContext.Result = ModelBindingResult.Success(ranges);
            return Task.CompletedTask;
        }
    }
}
