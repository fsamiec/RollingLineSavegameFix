using RollingLineSavegameFix.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RollingLineSavegameFix.Services
{
    /// <summary>
    /// Reformatting objects and shit
    /// </summary>
    public class ReformatService : IReformatService
    {
        private readonly IMainModel _model;

        public ReformatService(IMainModel model)
        {
            _model = model;
        }

        public void Reformat()
        {
            var content = _model.FileContent;
            content = content.Replace(Environment.NewLine, "");
            content = content.Replace("QuickMod", $"{Environment.NewLine}QuickMod", StringComparison.OrdinalIgnoreCase);
            _model.FileContent = content;
        }
    }
}

