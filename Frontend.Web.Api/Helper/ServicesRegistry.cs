using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using ADO;
//changed
using ADO.NET;
using Cos.BCS.Infrastructure.UnitOfWork;
using Infrastructure.DiagnosticTools;
using Infrastructure.Grid;
using Infrastructure.Pivot;
using Infrastructure.Query;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;

namespace Frontend.Web.Api.Helper
{
    public class ServicesRegistry : StructureMap.Registry
    {
        public ServicesRegistry(IConfiguration configuration)
        {
            For<IDataContext>().Use<AdoNetContext>();
            For<IUnitOfWork>().Use<AdoNetUnitOfWork>();
            For<IConnectionFactory>().Use<AppConfigConnectionFactory>()
                .Ctor<string>("connectionString")
                .Is(configuration.GetConnectionString("CosDB"))
                .Ctor<string>("providerName")
                .Is("System.Data.SqlClient");

            For<IDataContextCache>().Use<AdoNetContextCache>();
            //For<IConnectionFactoryCache>().Use<AppConfigConnectionFactoryCache>()
            //    .Ctor<string>("connectionString")
            //    .Is(configuration.GetConnectionString("CosDBCache"))
            //    .Ctor<string>("providerName")
            //    .Is("System.Data.SqlClient");

            ForSingletonOf<IGridHandler>().Use<GridHandler>();
            ForSingletonOf<IPivotHandler>().Use<PivotHandler>();
            ForSingletonOf<CodeExecutionMonitor.ILogMeasuremnetWriter>().Use<DefaultLogMeasuremnetWriter>()
               .Ctor<bool>("enabled")
               .Is(configuration.GetSection("SeriLog").GetValue<bool>("Enabled", false))
               .Ctor<string>("logpath")
               .Is(configuration.GetSection("SeriLog").GetValue<string>("Path", string.Empty));
            Scan(scanner =>
            {
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
                scanner.WithDefaultConventions();
            });

            //Scan(scanner =>
            //{
            //    scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
            //    scanner.WithDefaultConventions();
            //    scanner.IncludeNamespaceContainingType<BidHeaderQueryHandler>();
            //    scanner.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
            //});

            //For<IEmailSender>().Use<EmailSender>().Ctor<EmailConfiguration>("emailConfiguration").Is(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            For<MetaDataHelper>().Use<MetaDataHelper>();

        }
    }
}
