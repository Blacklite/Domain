using Blacklite.Framework.Domain.Process;
using Blacklite.Framework.Domain.Process.Requests;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using System.Collections.Generic;
using System;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    public class MyEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    [ServiceDescriptor]
    public class MyEntityStore
    {
        public Dictionary<string, MyEntity> Items { get; } = new Dictionary<string, MyEntity>();
    }

    [ServiceDescriptor(typeof(IProcessContextProvider))]
    class ProcessContextProvider : IProcessContextProvider
    {
        class ProcessContext : IProcessContext
        {
            public IServiceProvider ProcessServices { get; set; }
        }

        HttpContext _context;
        public ProcessContextProvider(IContextAccessor<HttpContext> contextAccessor)
        {
            _context = contextAccessor.Value;
        }
        public IProcessContext GetContextFor<T>(T instance)
        {
            return new ProcessContext() { ProcessServices = _context.RequestServices };
        }
    }

    public class MyEntityController : Controller
    {
        private MyEntityStore _store;
        public MyEntityController(MyEntityStore store)
        {
            _store = store;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var handler = Request.HttpContext.RequestServices.GetService<ISaveRequestHandler<MyEntity>>();

            var response = await handler.Handle(new SaveRequest<MyEntity>(
                new MyEntity()
                {
                    Name = "XYZ",
                    Id = "1",
                    Email = "bogus@test.com"
                })
            );

            if (response.Success)
            {
                var a = 1 + 1;
            }

            return View(response);
        }
    }
}
