using RollingLineSavegameFix.Model;
using System;

namespace RollingLineSavegameFix.Services
{
    public interface IReformatObjectsService : IReformatService
    {

    }
    public class ReformatObjectsService : IReformatObjectsService
    {
        public void Reformat()
        {
            //Alle 32 Kommas verliebt sich ein Object in den Garbage Collector
        }
    }


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

