using Microsoft.Xrm.Sdk;


namespace PMS
{
    public class SetScheduledDate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            
            ITracingService tracingService =
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

           
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

           
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                
                Entity entity = (Entity)context.InputParameters["Target"];

              
                IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                   
                    if (context.MessageName != "pms_SetScheduleddate") return;

                    if (entity is null || entity.LogicalName != "pms_inspection") return;

                    tracingService.Trace("SetScheduledDate plugin is executing.");

                    if (entity.Attributes.Contains("pms_scheduleddate"))
                    {
                        DateTime currentDateTime = entity.GetAttributeValue<DateTime>("pms_scheduleddate");
                        var year = currentDateTime.Year;
                        var month = currentDateTime.Month;
                        var day = currentDateTime.Day;
                        var time = currentDateTime.TimeOfDay;

                         #region update
                            Entity updatedEntity = new Entity();
                            updatedEntity.Id = entity.Id;
                            updatedEntity.LogicalName = entity.LogicalName;
                            updatedEntity["pms_comments"] = $"Year: {year} Month: {month} day: {day} time: {time}";

                            service.Update(updatedEntity);
                        #endregion update

                        tracingService.Trace($"Year: {year} Month: {month} day: {day} time: {time}");

                    }
                }

                catch (Exception ex)
                {
                    tracingService.Trace(ex.Message);
                    throw new InvalidPluginExecutionException("An error occurred in SetScheduledDate Plugin.", ex);
                }

            }
        }

    }
}
