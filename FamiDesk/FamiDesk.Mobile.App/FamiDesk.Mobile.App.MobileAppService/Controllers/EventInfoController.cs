﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using FamiDesk.Mobile.App.MobileAppService.DataObjects;
using FamiDesk.Mobile.App.MobileAppService.Models;
using System;

namespace FamiDesk.Mobile.App.MobileAppService.Controllers
{
    // TODO: Uncomment [Authorize] attribute to require user be authenticated to access Event(s).
    // [Authorize]
    public class EventInfoController : TableController<EventInfo>
    {
        MasterDetailContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MasterDetailContext();
            DomainManager = new EntityDomainManager<EventInfo>(context, Request);
        }

		// GET tables/EventInfo
		public IQueryable<EventInfo> GetAllEventInfos()
        {
            return Query();
        }

		// GET tables/EventInfo/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public SingleResult<EventInfo> GetEventInfo(string id)
        {
            return Lookup(id);
        }

		// PATCH tables/EventInfo/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task<EventInfo> PatchEventInfo(string id, Delta<EventInfo> patch)
        {
            return UpdateAsync(id, patch);
        }

		// POST tables/EventInfo
		public async Task<IHttpActionResult> PostEventInfo(EventInfo item)
        {
			EventInfo current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new {id = current.Id}, current);
        }

		// DELETE tables/EventInfo/48D68C86-6EA6-4C25-AA33-223FC9A27959
		public Task DeleteEventInfo(string id)
        {
            return DeleteAsync(id);
        }
    }
}