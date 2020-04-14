﻿namespace OfficeManager.Services
{
    using System.Threading.Tasks;

    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
